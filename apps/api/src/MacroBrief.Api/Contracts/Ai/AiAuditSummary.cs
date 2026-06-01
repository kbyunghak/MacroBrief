public record AiAuditSummary(
    int WindowSize,
    int TotalLogs,
    int FallbackUsedCount,
    double FallbackRate,
    bool FallbackRateWarning,
    int BlockedTermDetections,
    IReadOnlyList<string> TopBlockedTerms,
    IReadOnlyList<string> TopFailureCodes);
