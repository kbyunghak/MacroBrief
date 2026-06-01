public class InMemoryPortfolioInsightsService : IPortfolioInsightsService
{
    private readonly IMappingRulesProvider _mappingRulesProvider;
    private readonly IAiExplanationService _aiExplanationService;

    public InMemoryPortfolioInsightsService(IMappingRulesProvider mappingRulesProvider, IAiExplanationService aiExplanationService)
    {
        _mappingRulesProvider = mappingRulesProvider;
        _aiExplanationService = aiExplanationService;
    }

    public IReadOnlyList<ImpactCard> GetImpactCards(IReadOnlyList<Holding> holdings, IReadOnlyList<string>? symbols = null)
    {
        var filtered = symbols is { Count: > 0 }
            ? holdings.Where(h => symbols.Contains(h.Symbol, StringComparer.OrdinalIgnoreCase)).ToList()
            : holdings.ToList();

        return filtered
            .Take(5)
            .Select(h => BuildImpactCard(h.Symbol))
            .ToList();
    }

    public IReadOnlyList<LiveAlert> GetLiveAlerts(IReadOnlyList<Holding> holdings, int limit = 20)
    {
        var sanitizedLimit = Math.Clamp(limit, 1, 100);
        var now = DateTime.UtcNow;
        return holdings
            .Take(sanitizedLimit)
            .Select((h, i) => BuildLiveAlert(h.Symbol, i, now))
            .ToList();
    }

    public IReadOnlyList<MacroMapItem> GetMacroMap(IReadOnlyList<Holding> holdings)
    {
        var categoryCounts = holdings
            .Select(h => GetCategory(h.Symbol))
            .GroupBy(c => c)
            .Select(g => new MacroMapItem(g.Key, g.Count(), "flat"))
            .OrderByDescending(x => x.RelatedHoldingsCount)
            .ThenBy(x => x.Category)
            .ToList();

        return categoryCounts;
    }

    private ImpactCard BuildImpactCard(string symbol)
    {
        var category = GetCategory(symbol);
        var (headline, impactLevel, macroFactor, exposurePath, score) = category switch
        {
            "Semiconductors" => ("US chip policy headline flow increased", "high", "ai_semiconductors", "semiconductor demand expectations", 6),
            "Energy" => ("Crude oil moved higher intraday", "medium", "oil_energy", "energy price volatility", 5),
            "Interest Rates" => ("Treasury yields showed intraday volatility", "medium", "interest_rates", "funding and valuation assumptions", 5),
            _ => ("Macro headlines remain mixed", "low", "broad_macro", "overall risk sentiment", 3)
        };

        var candidate = $"This update may be relevant because changes in {macroFactor} can influence {symbol} through {exposurePath}.";
        var reason = _aiExplanationService.BuildGuardedExplanation(symbol, macroFactor, exposurePath, candidate, score);

        return new ImpactCard(symbol, headline, impactLevel, reason);
    }

    private LiveAlert BuildLiveAlert(string symbol, int index, DateTime now)
    {
        var category = GetCategory(symbol);
        var sourceName = category switch
        {
            "Semiconductors" => "Tech Policy Wire",
            "Energy" => "Energy Market Desk",
            "Interest Rates" => "Rates Monitor",
            _ => "Macro Update Desk"
        };
        var sourceUrl = category switch
        {
            "Semiconductors" => "https://example.com/tech-policy",
            "Energy" => "https://example.com/energy-market",
            "Interest Rates" => "https://example.com/rates-monitor",
            _ => "https://example.com/macro-desk"
        };

        return new LiveAlert(
            $"alert-{symbol.ToLowerInvariant()}-{index + 1}",
            now.AddMinutes(-(index + 1) * 3),
            category,
            index == 0 ? "watch" : "info",
            $"{symbol}: {category} event relevance updated.",
            sourceName,
            sourceUrl);
    }

    private string GetCategory(string symbol)
    {
        return GetCategory(symbol, _mappingRulesProvider.GetExposureTags(symbol));
    }

    private static string GetCategory(string symbol, IReadOnlyList<string> tags)
    {
        if (tags.Any(t => t.Contains("ai_semiconductors", StringComparison.OrdinalIgnoreCase)))
        {
            return "Semiconductors";
        }

        if (tags.Any(t => t.Contains("oil_energy", StringComparison.OrdinalIgnoreCase) || t.Contains("opec", StringComparison.OrdinalIgnoreCase)))
        {
            return "Energy";
        }

        if (tags.Any(t => t.Contains("interest_rates", StringComparison.OrdinalIgnoreCase) || t.Contains("consumer_credit", StringComparison.OrdinalIgnoreCase)))
        {
            return "Interest Rates";
        }

        return symbol switch
        {
            "NVDA" or "AMD" or "TSM" => "Semiconductors",
            "XOM" or "CVX" or "SLB" => "Energy",
            "JPM" or "BAC" or "SOFI" => "Interest Rates",
            _ => "Broad Macro"
        };
    }
}
