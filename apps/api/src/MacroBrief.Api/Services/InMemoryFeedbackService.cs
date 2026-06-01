public class InMemoryFeedbackService : IFeedbackService
{
    private readonly List<RelevanceFeedbackItem> _items = [];

    public void Add(RelevanceFeedbackItem item)
    {
        _items.Add(item);
    }

    public IReadOnlyList<RelevanceFeedbackItem> GetAll()
    {
        return _items.OrderByDescending(x => x.CreatedAtUtc).ToList();
    }
}
