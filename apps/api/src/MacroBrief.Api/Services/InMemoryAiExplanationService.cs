using System.Text.Json;

public class InMemoryAiExplanationService : IAiExplanationService
{
    private readonly string _promptVersion = "v1";
    private readonly int _maxLengthChars = 220;
    private readonly string _requiredPhrase = "may be relevant because";
    private readonly List<string> _bannedPatterns = [];
    private readonly List<string> _fallbackTemplates = [];
    protected readonly List<AiExplanationAuditItem> Logs = [];

    public InMemoryAiExplanationService(IHostEnvironment environment)
    {
        var root = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "..", ".."));
        var path = Path.Combine(root, "docs", "ai_guardrails.v1.json");
        if (!File.Exists(path))
        {
            _bannedPatterns.AddRange(["buy", "sell", "price target", "will go up", "will go down", "bullish", "bearish"]);
            _fallbackTemplates.Add("This update may be relevant to {symbol} because changes in {macro_factor} can affect {exposure_path}.");
            return;
        }

        var json = File.ReadAllText(path);
        var doc = JsonDocument.Parse(json).RootElement;

        if (doc.TryGetProperty("max_length_chars", out var maxChars))
        {
            _maxLengthChars = maxChars.GetInt32();
        }

        if (doc.TryGetProperty("required_phrase", out var requiredPhrase))
        {
            _requiredPhrase = requiredPhrase.GetString() ?? _requiredPhrase;
        }

        if (doc.TryGetProperty("banned_patterns", out var bannedPatterns))
        {
            _bannedPatterns.AddRange(bannedPatterns.EnumerateArray().Select(x => x.GetString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        if (doc.TryGetProperty("fallback_templates", out var fallbackTemplates))
        {
            _fallbackTemplates.AddRange(fallbackTemplates.EnumerateArray().Select(x => x.GetString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }

    public string BuildGuardedExplanation(string symbol, string macroFactor, string exposurePath, string candidateText, int score)
    {
        var attemptText = candidateText;
        var regenerationCount = 0;
        const int maxRegeneration = 2;
        var blockedTerms = new List<string>();
        var failureCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var attempt = 0; attempt <= maxRegeneration; attempt++)
        {
            blockedTerms = FindBlockedTerms(attemptText);
            failureCodes = GetFailureCodes(attemptText, blockedTerms);
            if (failureCodes.Count == 0)
            {
                break;
            }

            if (attempt == maxRegeneration)
            {
                break;
            }

            regenerationCount++;
            attemptText = RegenerateCandidateText(symbol, macroFactor, exposurePath, attemptText);
        }

        blockedTerms = FindBlockedTerms(attemptText);
        failureCodes = GetFailureCodes(attemptText, blockedTerms);
        var fallbackUsed = failureCodes.Count > 0;
        var outputText = fallbackUsed ? BuildFallback(symbol, macroFactor, exposurePath) : attemptText;
        var confidence = GetConfidence(score);

        AddAuditLog(new AiExplanationAuditItem(
            Symbol: symbol,
            MacroFactor: macroFactor,
            PromptVersion: _promptVersion,
            OutputText: outputText,
            ValidationFailureCodes: failureCodes.ToList(),
            BlockedTermsDetected: blockedTerms,
            RegenerationCount: regenerationCount,
            FallbackUsed: fallbackUsed,
            ConfidenceLabel: confidence,
            CreatedAtUtc: DateTime.UtcNow));

        return outputText;
    }

    public IReadOnlyList<AiExplanationAuditItem> GetAuditLogs()
    {
        return ReadAuditLogs().OrderByDescending(x => x.CreatedAtUtc).ToList();
    }

    protected virtual void AddAuditLog(AiExplanationAuditItem item)
    {
        Logs.Add(item);
    }

    protected virtual IReadOnlyList<AiExplanationAuditItem> ReadAuditLogs()
    {
        return Logs;
    }

    private List<string> FindBlockedTerms(string text)
    {
        return _bannedPatterns
            .Where(p => text.Contains(p, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private HashSet<string> GetFailureCodes(string text, IReadOnlyList<string> blockedTerms)
    {
        var failureCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (blockedTerms.Count > 0) failureCodes.Add("banned_terms");
        if (text.Length > _maxLengthChars) failureCodes.Add("too_long");
        if (!text.Contains(_requiredPhrase, StringComparison.OrdinalIgnoreCase)) failureCodes.Add("missing_required_phrase");
        return failureCodes;
    }

    private string RegenerateCandidateText(string symbol, string macroFactor, string exposurePath, string text)
    {
        var cleaned = text;
        foreach (var banned in _bannedPatterns)
        {
            cleaned = cleaned.Replace(banned, string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        if (!cleaned.Contains(_requiredPhrase, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = $"This update may be relevant because changes in {macroFactor} can influence {symbol} through {exposurePath}.";
        }

        cleaned = cleaned.Trim();
        if (cleaned.Length > _maxLengthChars)
        {
            cleaned = cleaned[.._maxLengthChars];
        }

        return cleaned;
    }

    private string BuildFallback(string symbol, string macroFactor, string exposurePath)
    {
        var template = _fallbackTemplates.FirstOrDefault()
            ?? "This update may be relevant to {symbol} because changes in {macro_factor} can affect {exposure_path}.";

        var text = template
            .Replace("{symbol}", symbol, StringComparison.OrdinalIgnoreCase)
            .Replace("{macro_factor}", macroFactor, StringComparison.OrdinalIgnoreCase)
            .Replace("{exposure_path}", exposurePath, StringComparison.OrdinalIgnoreCase);

        return text.Length <= _maxLengthChars ? text : text[.._maxLengthChars];
    }

    private static string GetConfidence(int score)
    {
        if (score >= 6) return "high";
        if (score >= 4) return "medium";
        return "low";
    }
}
