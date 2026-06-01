public record KpiEventsSummary(
    int TotalEvents,
    IReadOnlyDictionary<string, int> EventTypeCounts,
    DateTime? LatestOccurredAtUtc);
