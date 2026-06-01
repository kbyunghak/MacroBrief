public interface IAiExplanationService
{
    string BuildGuardedExplanation(string symbol, string macroFactor, string exposurePath, string candidateText, int score);
    IReadOnlyList<AiExplanationAuditItem> GetAuditLogs();
}
