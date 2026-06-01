public static class AiGuardrailsEndpoints
{
    public static void MapAiGuardrailsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/internal/ai/audit", (int? limit, IAiExplanationService aiExplanationService) =>
        {
            var take = Math.Clamp(limit ?? 20, 1, 100);
            var logs = aiExplanationService.GetAuditLogs().Take(take).ToList();
            return Results.Ok(ApiResponse<IEnumerable<AiExplanationAuditItem>>.Ok(logs));
        });

        app.MapGet("/api/v1/internal/ai/audit/summary", (int? window, IAiExplanationService aiExplanationService) =>
        {
            var logs = aiExplanationService.GetAuditLogs();
            var windowSize = Math.Clamp(window ?? 50, 1, 500);
            var windowLogs = logs.Take(windowSize).ToList();
            var topBlockedTerms = windowLogs
                .SelectMany(x => x.BlockedTermsDetected)
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var topFailureCodes = windowLogs
                .SelectMany(x => x.ValidationFailureCodes)
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var fallbackUsedCount = windowLogs.Count(x => x.FallbackUsed);
            var fallbackRate = windowLogs.Count == 0 ? 0 : (double)fallbackUsedCount / windowLogs.Count;
            var fallbackRateLevel = fallbackRate switch
            {
                >= 0.4 => "high",
                >= 0.2 => "medium",
                _ => "low"
            };

            var payload = new AiAuditSummary(
                WindowSize: windowSize,
                TotalLogs: windowLogs.Count,
                FallbackUsedCount: fallbackUsedCount,
                FallbackRate: Math.Round(fallbackRate, 3),
                FallbackRateLevel: fallbackRateLevel,
                FallbackRateWarning: fallbackRate >= 0.4,
                BlockedTermDetections: windowLogs.Sum(x => x.BlockedTermsDetected.Count),
                TopBlockedTerms: topBlockedTerms,
                TopFailureCodes: topFailureCodes);

            return Results.Ok(ApiResponse<AiAuditSummary>.Ok(payload));
        });
    }
}
