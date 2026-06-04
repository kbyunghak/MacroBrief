public class LocalJsonFeedbackService : IFeedbackService
{
    private readonly JsonFileStore<RelevanceFeedbackItem> _store;

    public LocalJsonFeedbackService(LocalJsonStorageOptions options)
    {
        _store = new JsonFileStore<RelevanceFeedbackItem>(Path.Combine(options.DataDirectory, "relevance-feedback.json"));
    }

    public void Add(RelevanceFeedbackItem item)
    {
        _store.Update(current =>
        {
            current.Add(item);
            return current;
        });
    }

    public IReadOnlyList<RelevanceFeedbackItem> GetAll()
    {
        return _store.ReadAll().OrderByDescending(x => x.CreatedAtUtc).ToList();
    }
}
