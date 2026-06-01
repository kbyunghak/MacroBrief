public record LiveAlert(
    string Id,
    DateTime CreatedAtUtc,
    string Category,
    string Severity,
    string Message);
