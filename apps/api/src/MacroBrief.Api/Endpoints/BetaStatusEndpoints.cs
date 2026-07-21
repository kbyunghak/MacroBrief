public static class BetaStatusEndpoints
{
    public static void MapBetaStatusEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/internal/beta/status", (IBetaStatusService betaStatusService) =>
            Results.Ok(ApiResponse<BetaStatus>.Ok(betaStatusService.GetStatus())));
    }
}
