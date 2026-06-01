public class InMemoryKpiEventService : IKpiEventService
{
    private readonly List<KpiEventItem> _items = [];

    public void Add(KpiEventItem item)
    {
        _items.Add(item);
    }

    public IReadOnlyList<KpiEventItem> GetRecent(int limit = 100)
    {
        var take = Math.Clamp(limit, 1, 1000);
        return _items
            .OrderByDescending(x => x.IngestedAtUtc)
            .Take(take)
            .ToList();
    }
}
