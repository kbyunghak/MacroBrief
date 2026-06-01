public class InMemoryPortfolioInsightsService : IPortfolioInsightsService
{
    private readonly IMappingRulesProvider _mappingRulesProvider;

    public InMemoryPortfolioInsightsService(IMappingRulesProvider mappingRulesProvider)
    {
        _mappingRulesProvider = mappingRulesProvider;
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
        return GetCategory(symbol) switch
        {
            "Semiconductors" => new ImpactCard(symbol, "US chip policy headline flow increased", "high", "Policy headlines can quickly change demand and export expectations."),
            "Energy" => new ImpactCard(symbol, "Crude oil moved higher intraday", "medium", "Energy price direction may affect earnings sensitivity."),
            "Interest Rates" => new ImpactCard(symbol, "Treasury yields showed intraday volatility", "medium", "Rate expectations can influence funding and valuation assumptions."),
            _ => new ImpactCard(symbol, "Macro headlines remain mixed", "low", "Broader macro shifts may still impact risk sentiment.")
        };
    }

    private LiveAlert BuildLiveAlert(string symbol, int index, DateTime now)
    {
        var category = GetCategory(symbol);
        return new LiveAlert(
            $"alert-{symbol.ToLowerInvariant()}-{index + 1}",
            now.AddMinutes(-(index + 1) * 3),
            category,
            index == 0 ? "watch" : "info",
            $"{symbol}: {category} event relevance updated.");
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
