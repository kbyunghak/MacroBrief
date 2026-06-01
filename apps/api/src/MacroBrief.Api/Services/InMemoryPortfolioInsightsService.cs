public class InMemoryPortfolioInsightsService : IPortfolioInsightsService
{
    public IReadOnlyList<ImpactCard> GetImpactCards(IReadOnlyList<Holding> holdings)
    {
        return holdings
            .Take(5)
            .Select(BuildImpactCard)
            .ToList();
    }

    public IReadOnlyList<LiveAlert> GetLiveAlerts(IReadOnlyList<Holding> holdings)
    {
        var now = DateTime.UtcNow;
        return holdings
            .Take(5)
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

    private static ImpactCard BuildImpactCard(Holding holding)
    {
        return GetCategory(holding.Symbol) switch
        {
            "Semiconductors" => new ImpactCard(holding.Symbol, "US chip policy headline flow increased", "high", "Policy headlines can quickly change demand and export expectations."),
            "Energy" => new ImpactCard(holding.Symbol, "Crude oil moved higher intraday", "medium", "Energy price direction may affect earnings sensitivity."),
            "Interest Rates" => new ImpactCard(holding.Symbol, "Treasury yields showed intraday volatility", "medium", "Rate expectations can influence funding and valuation assumptions."),
            _ => new ImpactCard(holding.Symbol, "Macro headlines remain mixed", "low", "Broader macro shifts may still impact risk sentiment.")
        };
    }

    private static LiveAlert BuildLiveAlert(string symbol, int index, DateTime now)
    {
        var category = GetCategory(symbol);
        return new LiveAlert(
            $"alert-{symbol.ToLowerInvariant()}-{index + 1}",
            now.AddMinutes(-(index + 1) * 3),
            category,
            index == 0 ? "watch" : "info",
            $"{symbol}: {category} event relevance updated.");
    }

    private static string GetCategory(string symbol)
    {
        return symbol switch
        {
            "NVDA" or "AMD" or "TSM" => "Semiconductors",
            "XOM" or "CVX" or "SLB" => "Energy",
            "JPM" or "BAC" or "SOFI" => "Interest Rates",
            _ => "Broad Macro"
        };
    }
}
