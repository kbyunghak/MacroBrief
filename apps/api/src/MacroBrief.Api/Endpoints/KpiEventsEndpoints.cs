public static class KpiEventsEndpoints
{
    private static readonly HashSet<string> AllowedEventTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "app_open",
        "dashboard_refresh",
        "holding_add",
        "holding_remove",
        "impact_feedback",
        "alert_view",
        "source_click"
    };

    public static void MapKpiEventsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/events", (KpiEventRequest request, IKpiEventService service) =>
        {
            if (string.IsNullOrWhiteSpace(request.EventId) ||
                string.IsNullOrWhiteSpace(request.EventType) ||
                string.IsNullOrWhiteSpace(request.UserId) ||
                string.IsNullOrWhiteSpace(request.SessionId))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_request", "event_id, event_type, user_id, and session_id are required."));
            }

            if (!AllowedEventTypes.Contains(request.EventType))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_event_type", "Unsupported event_type."));
            }

            var item = new KpiEventItem(
                EventId: request.EventId.Trim(),
                EventType: request.EventType.Trim(),
                UserId: request.UserId.Trim(),
                OccurredAtUtc: request.OccurredAtUtc,
                SessionId: request.SessionId.Trim(),
                Symbol: request.Symbol?.Trim().ToUpperInvariant(),
                NewsEventId: request.NewsEventId?.Trim(),
                Feedback: request.Feedback?.Trim().ToLowerInvariant(),
                ReasonTag: request.ReasonTag?.Trim(),
                SourceName: request.SourceName?.Trim(),
                SourceUrl: request.SourceUrl?.Trim(),
                Meta: request.Meta,
                IngestedAtUtc: DateTime.UtcNow);

            service.Add(item);
            return Results.Created("/api/v1/events", ApiResponse<object>.Ok(new { accepted = true }));
        });

        app.MapGet("/api/v1/internal/events", (int? limit, IKpiEventService service) =>
        {
            var items = service.GetRecent(limit ?? 100);
            return Results.Ok(ApiResponse<IEnumerable<KpiEventItem>>.Ok(items));
        });

        app.MapGet("/api/v1/internal/events/summary", (int? window, IKpiEventService service) =>
        {
            var take = Math.Clamp(window ?? 200, 1, 1000);
            var items = service.GetRecent(take);
            var counts = items
                .GroupBy(x => x.EventType, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

            var summary = new KpiEventsSummary(
                TotalEvents: items.Count,
                EventTypeCounts: counts,
                LatestOccurredAtUtc: items.Count == 0 ? null : items.Max(x => x.OccurredAtUtc));

            return Results.Ok(ApiResponse<KpiEventsSummary>.Ok(summary));
        });

        app.MapGet("/api/v1/internal/events/weekly-rollup", (int? days, IKpiEventService service, IAiExplanationService aiExplanationService) =>
        {
            var windowDays = Math.Clamp(days ?? 7, 1, 30);
            var fromUtc = DateTime.UtcNow.AddDays(-windowDays);
            var events = service.GetRecent(5000).Where(x => x.OccurredAtUtc >= fromUtc).ToList();
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
                    UserId = g.Key,
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

            var aiLogs = aiExplanationService.GetAuditLogs()
                .Where(x => x.CreatedAtUtc >= fromUtc)
                .ToList();
            var explanationPolicyViolationRate = aiLogs.Count == 0
                ? 0
                : (double)aiLogs.Count(x => x.ValidationFailureCodes.Count > 0) / aiLogs.Count;

            var kpiHealth = relevancePositiveRatio switch
            {
                >= 0.7 => "green",
                >= 0.5 => "yellow",
                _ => "red"
            };
            var recommendation = kpiHealth switch
            {
                "green" => "proceed",
                "yellow" => "iterate",
                _ => "reposition"
            };

            var rollup = new BetaWeeklyRollup(
                WeekStartDate: DateOnly.FromDateTime(fromUtc.Date),
                CohortSize: cohortSize,
                WeeklyActiveUsers: weeklyActiveUsers,
                D7RetentionRate: Math.Round(d7RetentionRate, 3),
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
                Notes: "D7RetentionRate is estimated from app_open first/last seen timestamps in the selected window.");

            return Results.Ok(ApiResponse<BetaWeeklyRollup>.Ok(rollup));
        });
    }
}
