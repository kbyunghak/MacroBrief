public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/dashboard/summary", (IHoldingsService holdingsService) =>
        {
            var payload = new DashboardSummary(
                LastUpdatedAt: DateTime.UtcNow,
                HoldingsCount: holdingsService.GetAll().Count,
                RelatedUpdatesCount: 12,
                TopExposureCategories: ["Interest Rates", "Oil & Geopolitics", "AI / Semiconductors"],
                MorningBrief:
                [
                    "Fed tone remains hawkish.",
                    "10-year Treasury yields moved higher.",
                    "Oil rose on geopolitical supply concerns."
                ]);

            return Results.Ok(ApiResponse<DashboardSummary>.Ok(payload));
        });
    }
}
