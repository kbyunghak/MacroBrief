using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

public class InMemoryAiExplanationServiceTests
{
    [Fact]
    public void BuildGuardedExplanation_RegeneratesCandidate_WhenBannedTermsDetected()
    {
        var service = new InMemoryAiExplanationService(new StubHostEnvironment());

        var output = service.BuildGuardedExplanation(
            symbol: "NVDA",
            macroFactor: "ai_semiconductors",
            exposurePath: "semiconductor demand",
            candidateText: "This is a buy signal and the stock will go up.",
            score: 6);

        Assert.Contains("may be relevant", output, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("buy signal", output, StringComparison.OrdinalIgnoreCase);
        Assert.NotEmpty(service.GetAuditLogs());
        Assert.True(service.GetAuditLogs()[0].RegenerationCount > 0);
        Assert.False(service.GetAuditLogs()[0].FallbackUsed);
    }

    [Fact]
    public void BuildGuardedExplanation_KeepsCandidate_WhenValid()
    {
        var service = new InMemoryAiExplanationService(new StubHostEnvironment());
        const string candidate = "This update may be relevant because changes in ai_semiconductors can influence NVDA demand expectations.";

        var output = service.BuildGuardedExplanation(
            symbol: "NVDA",
            macroFactor: "ai_semiconductors",
            exposurePath: "demand expectations",
            candidateText: candidate,
            score: 6);

        Assert.Equal(candidate, output);
        Assert.False(service.GetAuditLogs()[0].FallbackUsed);
    }

    private sealed class StubHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "MacroBrief.Api.Tests";
        public string ContentRootPath { get; set; } = @"C:\Users\kbyun\Documents\MacroBrief\apps\api\src\MacroBrief.Api";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
