public record AiAuditSummary(
    int TotalLogs,
    int FallbackUsedCount,
    int BlockedTermDetections,
    IReadOnlyList<string> TopBlockedTerms);
