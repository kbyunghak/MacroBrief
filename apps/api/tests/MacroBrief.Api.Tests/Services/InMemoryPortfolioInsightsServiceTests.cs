public class InMemoryPortfolioInsightsServiceTests
{
    private static readonly IReadOnlyList<Holding> SeedHoldings =
    [
        new("NVDA"),
        new("XOM"),
        new("SOFI")
    ];

    [Fact]
    public void GetImpactCards_ReturnsPerHoldingCards()
    {
        var service = new InMemoryPortfolioInsightsService(new StubMappingRulesProvider(), new StubAiExplanationService());

        var cards = service.GetImpactCards(SeedHoldings);

        Assert.Equal(3, cards.Count);
        Assert.Contains(cards, c => c.Symbol == "NVDA" && c.ImpactLevel == "high");
        Assert.Contains(cards, c => c.Symbol == "XOM" && c.ImpactLevel == "medium");
    }

    [Fact]
    public void GetLiveAlerts_ReturnsStableAlertShape()
    {
        var service = new InMemoryPortfolioInsightsService(new StubMappingRulesProvider(), new StubAiExplanationService());

        var alerts = service.GetLiveAlerts(SeedHoldings);

        Assert.Equal(3, alerts.Count);
        Assert.All(alerts, a => Assert.StartsWith("alert-", a.Id));
        Assert.All(alerts, a => Assert.False(string.IsNullOrWhiteSpace(a.Message)));
    }

    [Fact]
    public void GetLiveAlerts_AppliesLimit()
    {
        var service = new InMemoryPortfolioInsightsService(new StubMappingRulesProvider(), new StubAiExplanationService());

        var alerts = service.GetLiveAlerts(SeedHoldings, limit: 2);

        Assert.Equal(2, alerts.Count);
    }

    [Fact]
    public void GetMacroMap_GroupsByCategory()
    {
        var service = new InMemoryPortfolioInsightsService(new StubMappingRulesProvider(), new StubAiExplanationService());

        var map = service.GetMacroMap(SeedHoldings);

        Assert.Contains(map, m => m.Category == "Semiconductors" && m.RelatedHoldingsCount == 1);
        Assert.Contains(map, m => m.Category == "Energy" && m.RelatedHoldingsCount == 1);
        Assert.Contains(map, m => m.Category == "Interest Rates" && m.RelatedHoldingsCount == 1);
    }

    [Fact]
    public void GetImpactCards_FiltersSymbolsWhenProvided()
    {
        var service = new InMemoryPortfolioInsightsService(new StubMappingRulesProvider(), new StubAiExplanationService());

        var cards = service.GetImpactCards(SeedHoldings, ["NVDA"]);

        Assert.Single(cards);
        Assert.Equal("NVDA", cards[0].Symbol);
    }

    private sealed class StubMappingRulesProvider : IMappingRulesProvider
    {
        public IReadOnlyList<string> GetExposureTags(string symbol) => [];
    }

    private sealed class StubAiExplanationService : IAiExplanationService
    {
        public string BuildGuardedExplanation(string symbol, string macroFactor, string exposurePath, string candidateText, int score)
            => candidateText;

        public IReadOnlyList<AiExplanationAuditItem> GetAuditLogs() => [];
    }
}
