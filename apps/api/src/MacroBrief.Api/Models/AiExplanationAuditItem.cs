public record AiExplanationAuditItem(
    string Symbol,
    string MacroFactor,
    string PromptVersion,
    string OutputText,
    IReadOnlyList<string> ValidationFailureCodes,
    IReadOnlyList<string> BlockedTermsDetected,
    int RegenerationCount,
    bool FallbackUsed,
    string ConfidenceLabel,
    DateTime CreatedAtUtc);
