public static class PortfolioInsightsEndpoints
{
    public static void MapPortfolioInsightsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/dashboard/impact-cards", () =>
        {
            var payload = new[]
            {
                new ImpactCard("NVDA", "US chip export rules discussed again", "high", "Export-policy changes can affect AI chip demand visibility."),
                new ImpactCard("XOM", "Brent crude moved above recent range", "medium", "Higher oil prices can support upstream energy earnings."),
                new ImpactCard("JPM", "Treasury yields climbed during the session", "medium", "Rate expectations may influence net interest margin outlook.")
            };

            return Results.Ok(ApiResponse<IEnumerable<ImpactCard>>.Ok(payload));
        });

        app.MapGet("/api/v1/dashboard/live-alerts", () =>
        {
            var now = DateTime.UtcNow;
            var payload = new[]
            {
                new LiveAlert("alert-1", now.AddMinutes(-8), "Rates", "info", "2Y yield moved +6 bps since US open."),
                new LiveAlert("alert-2", now.AddMinutes(-4), "Commodities", "watch", "WTI crude is up more than 1% intraday."),
                new LiveAlert("alert-3", now.AddMinutes(-2), "Policy", "watch", "Fed speaker remarks scheduled within the next hour.")
            };

            return Results.Ok(ApiResponse<IEnumerable<LiveAlert>>.Ok(payload));
        });

        app.MapGet("/api/v1/dashboard/macro-map", () =>
        {
            var payload = new[]
            {
                new MacroMapItem("Interest Rates", 3, "up"),
                new MacroMapItem("Energy", 2, "up"),
                new MacroMapItem("Semiconductors", 4, "flat")
            };

            return Results.Ok(ApiResponse<IEnumerable<MacroMapItem>>.Ok(payload));
        });
    }
}
