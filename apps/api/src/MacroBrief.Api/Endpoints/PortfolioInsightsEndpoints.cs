public static class PortfolioInsightsEndpoints
{
    public static void MapPortfolioInsightsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/impact-cards", (string? symbols, IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var symbolList = (symbols ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToUpperInvariant())
                .Distinct()
                .ToList();

            var payload = insightsService.GetImpactCards(holdingsService.GetAll(), symbolList);
            return Results.Ok(ApiResponse<IEnumerable<ImpactCard>>.Ok(payload));
        });

        app.MapGet("/api/v1/live-alerts", (int? limit, IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var payload = insightsService.GetLiveAlerts(holdingsService.GetAll(), limit ?? 20);
            return Results.Ok(ApiResponse<IEnumerable<LiveAlert>>.Ok(payload));
        });

        app.MapGet("/api/v1/macro-map", (IHoldingsService holdingsService, IPortfolioInsightsService insightsService) =>
        {
            var payload = insightsService.GetMacroMap(holdingsService.GetAll());
            return Results.Ok(ApiResponse<IEnumerable<MacroMapItem>>.Ok(payload));
        });
    }
}
