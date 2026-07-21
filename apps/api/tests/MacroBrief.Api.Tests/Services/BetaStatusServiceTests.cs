using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class BetaStatusServiceTests
{
    [Fact]
    public void GetStatus_WithNoEvents_ReturnsInsufficientData()
    {
        var kpiService = new InMemoryKpiEventService();
        var aiService = new InMemoryAiExplanationService(new StubHostEnvironment());
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "memory"
            })
            .Build();
        var environment = new StubHostEnvironment();
        var storageStatus = new StorageStatusService(configuration, environment);
        var localData = new LocalDataManagementService(configuration, environment);
        var service = new BetaStatusService(kpiService, aiService, storageStatus, localData);

        var status = service.GetStatus();

        Assert.Equal("memory", status.Storage.Mode);
        Assert.Equal(0, status.EventSummary.TotalEvents);
        Assert.Equal("insufficient_data", status.WeeklyRollup.KpiHealth);
        Assert.Equal("collect_more_data", status.WeeklyRollup.Recommendation);
        Assert.Equal(0, status.AiAuditSummary.TotalLogs);
    }

    [Fact]
    public void GetStatus_WithEnoughPositiveFeedback_ReturnsGreenProceed()
    {
        var kpiService = new InMemoryKpiEventService();
        var now = DateTime.UtcNow;
        for (var i = 0; i < 5; i++)
        {
            kpiService.Add(new KpiEventItem(
                EventId: $"evt-beta-status-{i}",
                EventType: "impact_feedback",
                UserId: $"usr-beta-status-{i}",
                OccurredAtUtc: now,
                SessionId: $"ses-beta-status-{i}",
                Symbol: "NVDA",
                NewsEventId: $"news-beta-status-{i}",
                Feedback: i < 4 ? "relevant" : "not_relevant",
                ReasonTag: null,
                SourceName: null,
                SourceUrl: null,
                Meta: null,
                IngestedAtUtc: now));
        }

        var aiService = new InMemoryAiExplanationService(new StubHostEnvironment());
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "memory"
            })
            .Build();
        var environment = new StubHostEnvironment();
        var storageStatus = new StorageStatusService(configuration, environment);
        var localData = new LocalDataManagementService(configuration, environment);
        var service = new BetaStatusService(kpiService, aiService, storageStatus, localData);

        var status = service.GetStatus();

        Assert.Equal(5, status.EventSummary.TotalEvents);
        Assert.Equal(5, status.WeeklyRollup.FeedbackSampleSize);
        Assert.Equal(0.8, status.WeeklyRollup.RelevancePositiveRatio);
        Assert.Equal("green", status.WeeklyRollup.KpiHealth);
        Assert.Equal("proceed", status.WeeklyRollup.Recommendation);
    }

    private sealed class StubHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "MacroBrief.Api.Tests";
        public string ContentRootPath { get; set; } = Path.Combine(
            Directory.GetCurrentDirectory(),
            "apps",
            "api",
            "src",
            "MacroBrief.Api");
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
