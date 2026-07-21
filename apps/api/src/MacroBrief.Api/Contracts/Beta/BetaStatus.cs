public record BetaStatus(
    StorageStatus Storage,
    KpiEventsSummary EventSummary,
    BetaWeeklyRollup WeeklyRollup,
    AiAuditSummary AiAuditSummary,
    LocalDataExport LocalData);
