public static class PortfolioInsightsEndpoints
{
    public static void MapPortfolioInsightsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/impact-cards", (IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var payload = insightsService.GetImpactCards(holdingsService.GetAll());
            return Results.Ok(ApiResponse<IEnumerable<ImpactCard>>.Ok(payload));
        });

        app.MapGet("/api/v1/live-alerts", (IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var payload = insightsService.GetLiveAlerts(holdingsService.GetAll());
            return Results.Ok(ApiResponse<IEnumerable<LiveAlert>>.Ok(payload));
        });

        app.MapGet("/api/v1/macro-map", (IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var payload = insightsService.GetMacroMap(holdingsService.GetAll());
            return Results.Ok(ApiResponse<IEnumerable<MacroMapItem>>.Ok(payload));
        });
    }
}
