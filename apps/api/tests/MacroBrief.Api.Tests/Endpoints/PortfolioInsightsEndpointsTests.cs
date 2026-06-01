using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class PortfolioInsightsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PortfolioInsightsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetImpactCards_ReturnsOkWithData()
    {
        var response = await _client.GetAsync("/api/v1/impact-cards");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        Assert.Equal("NVDA", items[0].GetProperty("symbol").GetString());
    }

    [Fact]
    public async Task GetImpactCards_WithSymbolsFilter_ReturnsOnlyRequestedSymbols()
    {
        var response = await _client.GetAsync("/api/v1/impact-cards?symbols=NVDA");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.Equal(1, items.GetArrayLength());
        Assert.Equal("NVDA", items[0].GetProperty("symbol").GetString());
    }

    [Fact]
    public async Task GetLiveAlerts_ReturnsOkWithData()
    {
        var response = await _client.GetAsync("/api/v1/live-alerts");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        Assert.Contains("alert-", items[0].GetProperty("id").GetString());
    }

    [Fact]
    public async Task GetLiveAlerts_WithLimit_ReturnsLimitedItems()
    {
        var response = await _client.GetAsync("/api/v1/live-alerts?limit=2");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.Equal(2, items.GetArrayLength());
    }

    [Fact]
    public async Task GetMacroMap_ReturnsOkWithData()
    {
        var response = await _client.GetAsync("/api/v1/macro-map");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        var categories = items.EnumerateArray().Select(x => x.GetProperty("category").GetString()).ToArray();
        Assert.Contains("Semiconductors", categories);
    }
}
