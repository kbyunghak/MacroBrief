public class LocalJsonKpiEventService : IKpiEventService
{
    private readonly JsonFileStore<KpiEventItem> _store;

    public LocalJsonKpiEventService(LocalJsonStorageOptions options)
    {
        _store = new JsonFileStore<KpiEventItem>(Path.Combine(options.DataDirectory, "kpi-events.json"));
    }

    public void Add(KpiEventItem item)
    {
        _store.Update(current =>
        {
            current.Add(item);
            return current;
        });
    }

    public IReadOnlyList<KpiEventItem> GetRecent(int limit = 100)
    {
        var take = Math.Clamp(limit, 1, 1000);
        return _store.ReadAll()
            .OrderByDescending(x => x.IngestedAtUtc)
            .Take(take)
            .ToList();
    }
}
