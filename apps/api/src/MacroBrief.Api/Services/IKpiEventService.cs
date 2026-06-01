public interface IKpiEventService
{
    void Add(KpiEventItem item);
    IReadOnlyList<KpiEventItem> GetRecent(int limit = 100);
}
