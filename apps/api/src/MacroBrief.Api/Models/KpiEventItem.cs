public record KpiEventItem(
    string EventId,
    string EventType,
    string UserId,
    DateTime OccurredAtUtc,
    string SessionId,
    string? Symbol,
    string? NewsEventId,
    string? Feedback,
    string? ReasonTag,
    string? SourceName,
    string? SourceUrl,
    IReadOnlyDictionary<string, string>? Meta,
    DateTime IngestedAtUtc);
