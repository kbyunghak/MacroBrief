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

        app.MapGet("/api/v1/internal/ai/audit/summary", (IAiExplanationService aiExplanationService) =>
        {
            var logs = aiExplanationService.GetAuditLogs();
            var topBlockedTerms = logs
                .SelectMany(x => x.BlockedTermsDetected)
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key)
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var payload = new AiAuditSummary(
                TotalLogs: logs.Count,
                FallbackUsedCount: logs.Count(x => x.FallbackUsed),
                BlockedTermDetections: logs.Sum(x => x.BlockedTermsDetected.Count),
                TopBlockedTerms: topBlockedTerms);

            return Results.Ok(ApiResponse<AiAuditSummary>.Ok(payload));
        });
    }
}
