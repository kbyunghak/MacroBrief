public record DashboardSummary(
    DateTime LastUpdatedAt,
    int HoldingsCount,
    int RelatedUpdatesCount,
    IEnumerable<string> TopExposureCategories,
    IEnumerable<string> MorningBrief);
