public class LocalJsonKpiEventServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void Add_PersistsEventAcrossServiceInstances()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonKpiEventService(options);

        firstService.Add(new KpiEventItem(
            EventId: "evt-local-json",
            EventType: "app_open",
            UserId: "usr-local-json",
            OccurredAtUtc: DateTime.UtcNow,
            SessionId: "ses-local-json",
            Symbol: null,
            NewsEventId: null,
            Feedback: null,
            ReasonTag: null,
            SourceName: null,
            SourceUrl: null,
            Meta: null,
            IngestedAtUtc: DateTime.UtcNow));

        var secondService = new LocalJsonKpiEventService(options);
        var events = secondService.GetRecent(10);

        Assert.Single(events);
        Assert.Equal("evt-local-json", events[0].EventId);
    }

    [Fact]
    public void GetRecent_ReturnsNewestEventsFirstAndAppliesLimit()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var service = new LocalJsonKpiEventService(options);

        service.Add(BuildEvent("evt-old", DateTime.UtcNow.AddMinutes(-10)));
        service.Add(BuildEvent("evt-new", DateTime.UtcNow));

        var events = service.GetRecent(1);

        Assert.Single(events);
        Assert.Equal("evt-new", events[0].EventId);
    }

    private static KpiEventItem BuildEvent(string eventId, DateTime ingestedAtUtc)
    {
        return new KpiEventItem(
            EventId: eventId,
            EventType: "app_open",
            UserId: "usr-local-json",
            OccurredAtUtc: ingestedAtUtc,
            SessionId: "ses-local-json",
            Symbol: null,
            NewsEventId: null,
            Feedback: null,
            ReasonTag: null,
            SourceName: null,
            SourceUrl: null,
            Meta: null,
            IngestedAtUtc: ingestedAtUtc);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dataDirectory))
        {
            Directory.Delete(_dataDirectory, recursive: true);
        }
    }
}
