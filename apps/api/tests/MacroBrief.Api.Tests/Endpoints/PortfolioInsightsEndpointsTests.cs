using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class PortfolioInsightsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PortfolioInsightsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetImpactCards_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/dashboard/impact-cards");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetLiveAlerts_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/dashboard/live-alerts");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMacroMap_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/dashboard/macro-map");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
