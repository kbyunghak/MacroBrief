using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class LocalJsonAiExplanationServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void BuildGuardedExplanation_PersistsAuditLogAcrossServiceInstances()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonAiExplanationService(new StubHostEnvironment(), options);

        var output = firstService.BuildGuardedExplanation(
            symbol: "NVDA",
            macroFactor: "ai_semiconductors",
            exposurePath: "semiconductor demand",
            candidateText: "This is a buy signal and the stock will go up.",
            score: 6);

        var secondService = new LocalJsonAiExplanationService(new StubHostEnvironment(), options);
        var logs = secondService.GetAuditLogs();

        Assert.Contains("may be relevant", output, StringComparison.OrdinalIgnoreCase);
        Assert.Single(logs);
        Assert.Equal("NVDA", logs[0].Symbol);
        Assert.Equal("ai_semiconductors", logs[0].MacroFactor);
        Assert.True(logs[0].RegenerationCount > 0);
        Assert.False(logs[0].FallbackUsed);
        Assert.True(File.Exists(Path.Combine(_dataDirectory, "ai-explanation-audit.json")));
    }

    public void Dispose()
    {
        if (Directory.Exists(_dataDirectory))
        {
            Directory.Delete(_dataDirectory, recursive: true);
        }
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
