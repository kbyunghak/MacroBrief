public class LocalJsonFeedbackServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void Add_PersistsFeedbackAcrossServiceInstances()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonFeedbackService(options);
        var createdAt = DateTime.UtcNow;

        firstService.Add(new RelevanceFeedbackItem(
            NewsEventId: "news-local-json",
            Symbol: "NVDA",
            Feedback: "relevant",
            CreatedAtUtc: createdAt));

        var secondService = new LocalJsonFeedbackService(options);
        var feedback = secondService.GetAll();

        Assert.Single(feedback);
        Assert.Equal("news-local-json", feedback[0].NewsEventId);
        Assert.Equal("NVDA", feedback[0].Symbol);
        Assert.Equal("relevant", feedback[0].Feedback);
    }

    [Fact]
    public void GetAll_ReturnsNewestFeedbackFirst()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var service = new LocalJsonFeedbackService(options);

        service.Add(new RelevanceFeedbackItem("news-old", "NVDA", "relevant", DateTime.UtcNow.AddMinutes(-10)));
        service.Add(new RelevanceFeedbackItem("news-new", "NVDA", "not_relevant", DateTime.UtcNow));

        var feedback = service.GetAll();

        Assert.Equal("news-new", feedback[0].NewsEventId);
        Assert.Equal("news-old", feedback[1].NewsEventId);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dataDirectory))
        {
            Directory.Delete(_dataDirectory, recursive: true);
        }
    }
}
