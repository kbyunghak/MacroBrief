public interface IPortfolioInsightsService
{
    IReadOnlyList<ImpactCard> GetImpactCards(IReadOnlyList<Holding> holdings, IReadOnlyList<string>? symbols = null);
    IReadOnlyList<LiveAlert> GetLiveAlerts(IReadOnlyList<Holding> holdings, int limit = 20);
    IReadOnlyList<MacroMapItem> GetMacroMap(IReadOnlyList<Holding> holdings);
}
