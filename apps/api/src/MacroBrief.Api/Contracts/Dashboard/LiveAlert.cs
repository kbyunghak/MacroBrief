public record LiveAlert(
    string Id,
    DateTime CreatedAtUtc,
    string Category,
    string Severity,
    string Message,
    string SourceName,
    string SourceUrl);
