public interface IFeedbackService
{
    void Add(RelevanceFeedbackItem item);
    IReadOnlyList<RelevanceFeedbackItem> GetAll();
}
