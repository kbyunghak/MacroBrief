public class BetaStatusService : IBetaStatusService
{
    private const int MinimumFeedbackSampleSize = 5;

    private readonly IKpiEventService _kpiEventService;
    private readonly IAiExplanationService _aiExplanationService;
    private readonly IStorageStatusService _storageStatusService;
    private readonly ILocalDataManagementService _localDataManagementService;

    public BetaStatusService(
        IKpiEventService kpiEventService,
        IAiExplanationService aiExplanationService,
        IStorageStatusService storageStatusService,
        ILocalDataManagementService localDataManagementService)
    {
        _kpiEventService = kpiEventService;
        _aiExplanationService = aiExplanationService;
        _storageStatusService = storageStatusService;
        _localDataManagementService = localDataManagementService;
    }

    public BetaStatus GetStatus(int eventWindow = 200, int rollupDays = 7, int aiAuditWindow = 50)
    {
        return new BetaStatus(
            _storageStatusService.GetStatus(),
            BuildEventSummary(eventWindow),
            BuildWeeklyRollup(rollupDays),
            BuildAiAuditSummary(aiAuditWindow),
            _localDataManagementService.ExportStatus());
    }

    private KpiEventsSummary BuildEventSummary(int eventWindow)
    {
        var take = Math.Clamp(eventWindow, 1, 1000);
        var events = _kpiEventService.GetRecent(take);
        var counts = events
            .GroupBy(x => x.EventType, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        return new KpiEventsSummary(
            TotalEvents: events.Count,
            EventTypeCounts: counts,
            LatestOccurredAtUtc: events.Count == 0 ? null : events.Max(x => x.OccurredAtUtc));
    }

    private BetaWeeklyRollup BuildWeeklyRollup(int rollupDays)
    {
        var windowDays = Math.Clamp(rollupDays, 1, 30);
        var fromUtc = DateTime.UtcNow.AddDays(-windowDays);
        var events = _kpiEventService.GetRecent(5000).Where(x => x.OccurredAtUtc >= fromUtc).ToList();
        var appOpenEvents = events.Where(x => x.EventType.Equals("app_open", StringComparison.OrdinalIgnoreCase)).ToList();
        var impactFeedbackEvents = events.Where(x => x.EventType.Equals("impact_feedback", StringComparison.OrdinalIgnoreCase)).ToList();
        var alertViewCount = events.Count(x => x.EventType.Equals("alert_view", StringComparison.OrdinalIgnoreCase));
        var sourceClickCount = events.Count(x => x.EventType.Equals("source_click", StringComparison.OrdinalIgnoreCase));

        var cohortSize = appOpenEvents.Select(x => x.UserId).Distinct(StringComparer.OrdinalIgnoreCase).Count();
        var weeklyActiveUsers = events.Select(x => x.UserId).Distinct(StringComparer.OrdinalIgnoreCase).Count();

        var positiveFeedback = impactFeedbackEvents.Count(x => x.Feedback == "relevant");
        var negativeFeedback = impactFeedbackEvents.Count(x => x.Feedback == "not_relevant");
        var totalFeedback = impactFeedbackEvents.Count;

        var relevancePositiveRatio = totalFeedback == 0 ? 0 : (double)positiveFeedback / totalFeedback;
        var falseRelevanceRate = totalFeedback == 0 ? 0 : (double)negativeFeedback / totalFeedback;
        var alertCtr = appOpenEvents.Count == 0 ? 0 : (double)alertViewCount / appOpenEvents.Count;
        var sourceClickRate = appOpenEvents.Count == 0 ? 0 : (double)sourceClickCount / appOpenEvents.Count;

        var appOpenByUser = appOpenEvents
            .GroupBy(x => x.UserId, StringComparer.OrdinalIgnoreCase)
            .Select(g => new
            {
                FirstSeen = g.Min(x => x.OccurredAtUtc),
                LastSeen = g.Max(x => x.OccurredAtUtc)
            })
            .ToList();
        var d7Eligible = appOpenByUser.Count(x => x.FirstSeen <= DateTime.UtcNow.AddDays(-7));
        var d7Retained = appOpenByUser.Count(x => x.LastSeen >= x.FirstSeen.AddDays(7));
        var d7RetentionRate = d7Eligible == 0 ? 0 : (double)d7Retained / d7Eligible;

        var feedbackThemes = impactFeedbackEvents
            .Where(x => !string.IsNullOrWhiteSpace(x.ReasonTag))
            .GroupBy(x => x.ReasonTag!, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        var aiLogs = _aiExplanationService.GetAuditLogs()
            .Where(x => x.CreatedAtUtc >= fromUtc)
            .ToList();
        var explanationPolicyViolationRate = aiLogs.Count == 0
            ? 0
            : (double)aiLogs.Count(x => x.ValidationFailureCodes.Count > 0) / aiLogs.Count;

        var kpiHealth = totalFeedback < MinimumFeedbackSampleSize
            ? "insufficient_data"
            : relevancePositiveRatio switch
        {
            >= 0.7 => "green",
            >= 0.5 => "yellow",
            _ => "red"
        };
        var recommendation = kpiHealth switch
        {
            "green" => "proceed",
            "yellow" => "iterate",
            "insufficient_data" => "collect_more_data",
            _ => "reposition"
        };

        return new BetaWeeklyRollup(
            WeekStartDate: DateOnly.FromDateTime(fromUtc.Date),
            CohortSize: cohortSize,
            WeeklyActiveUsers: weeklyActiveUsers,
            D7RetentionRate: Math.Round(d7RetentionRate, 3),
            FeedbackSampleSize: totalFeedback,
            RelevancePositiveRatio: Math.Round(relevancePositiveRatio, 3),
            AlertClickThroughRate: Math.Round(alertCtr, 3),
            SourceClickRate: Math.Round(sourceClickRate, 3),
            ExplanationPolicyViolationRate: Math.Round(explanationPolicyViolationRate, 3),
            KpiHealth: kpiHealth,
            Recommendation: recommendation,
            FalseRelevanceRate: Math.Round(falseRelevanceRate, 3),
            DuplicateAlertRate: 0,
            MissingSourceRate: 0,
            TopFeedbackThemes: feedbackThemes,
            RuleVersion: "v1",
            Notes: $"D7RetentionRate is estimated from app_open first/last seen timestamps in the selected window. KPI health requires at least {MinimumFeedbackSampleSize} feedback events.");
    }

    private AiAuditSummary BuildAiAuditSummary(int aiAuditWindow)
    {
        var windowSize = Math.Clamp(aiAuditWindow, 1, 500);
        var logs = _aiExplanationService.GetAuditLogs().Take(windowSize).ToList();
        var topBlockedTerms = logs
            .SelectMany(x => x.BlockedTermsDetected)
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(5)
            .Select(g => g.Key)
            .ToList();
        var topFailureCodes = logs
            .SelectMany(x => x.ValidationFailureCodes)
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(5)
            .Select(g => g.Key)
            .ToList();

        var fallbackUsedCount = logs.Count(x => x.FallbackUsed);
        var fallbackRate = logs.Count == 0 ? 0 : (double)fallbackUsedCount / logs.Count;
        var fallbackRateLevel = fallbackRate switch
        {
            >= 0.4 => "high",
            >= 0.2 => "medium",
            _ => "low"
        };

        return new AiAuditSummary(
            WindowSize: windowSize,
            TotalLogs: logs.Count,
            FallbackUsedCount: fallbackUsedCount,
            FallbackRate: Math.Round(fallbackRate, 3),
            FallbackRateLevel: fallbackRateLevel,
            FallbackRateWarning: fallbackRate >= 0.4,
            BlockedTermDetections: logs.Sum(x => x.BlockedTermsDetected.Count),
            TopBlockedTerms: topBlockedTerms,
            TopFailureCodes: topFailureCodes);
    }
}
