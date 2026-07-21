using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class BetaStatusEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BetaStatusEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBetaStatus_ReturnsStorageKpiRollupAndAiSummary()
    {
        var response = await _client.GetAsync("/api/v1/internal/beta/status");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.True(data.TryGetProperty("storage", out _));
        Assert.True(data.TryGetProperty("eventSummary", out _));
        Assert.True(data.TryGetProperty("weeklyRollup", out _));
        Assert.True(data.TryGetProperty("aiAuditSummary", out _));
        Assert.True(data.TryGetProperty("localData", out _));
    }
}
