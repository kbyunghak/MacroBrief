public interface IPortfolioInsightsService
{
    IReadOnlyList<ImpactCard> GetImpactCards(IReadOnlyList<Holding> holdings);
    IReadOnlyList<LiveAlert> GetLiveAlerts(IReadOnlyList<Holding> holdings);
    IReadOnlyList<MacroMapItem> GetMacroMap(IReadOnlyList<Holding> holdings);
}
