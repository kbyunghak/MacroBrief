using System.Text.Json;

public class InMemoryAiExplanationService : IAiExplanationService
{
    private readonly string _promptVersion = "v1";
    private readonly int _maxLengthChars = 220;
    private readonly string _requiredPhrase = "may be relevant because";
    private readonly List<string> _bannedPatterns = [];
    private readonly List<string> _fallbackTemplates = [];
    private readonly List<AiExplanationAuditItem> _logs = [];

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
        var blockedTerms = FindBlockedTerms(candidateText);
        var tooLong = candidateText.Length > _maxLengthChars;
        var hasRequiredPhrase = candidateText.Contains(_requiredPhrase, StringComparison.OrdinalIgnoreCase);
        var fallbackUsed = blockedTerms.Count > 0 || tooLong || !hasRequiredPhrase;
        var regenerationCount = fallbackUsed ? 1 : 0;
        var outputText = fallbackUsed ? BuildFallback(symbol, macroFactor, exposurePath) : candidateText;
        var confidence = GetConfidence(score);

        _logs.Add(new AiExplanationAuditItem(
            Symbol: symbol,
            MacroFactor: macroFactor,
            PromptVersion: _promptVersion,
            OutputText: outputText,
            BlockedTermsDetected: blockedTerms,
            RegenerationCount: regenerationCount,
            FallbackUsed: fallbackUsed,
            ConfidenceLabel: confidence,
            CreatedAtUtc: DateTime.UtcNow));

        return outputText;
    }

    public IReadOnlyList<AiExplanationAuditItem> GetAuditLogs()
    {
        return _logs.OrderByDescending(x => x.CreatedAtUtc).ToList();
    }

    private List<string> FindBlockedTerms(string text)
    {
        return _bannedPatterns
            .Where(p => text.Contains(p, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
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
