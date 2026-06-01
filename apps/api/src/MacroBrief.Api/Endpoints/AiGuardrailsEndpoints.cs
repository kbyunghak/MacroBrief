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
    }
}
