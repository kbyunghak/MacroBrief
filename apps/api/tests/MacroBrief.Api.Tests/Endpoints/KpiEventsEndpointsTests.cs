using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class KpiEventsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public KpiEventsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostEvent_ThenGetRecent_ReturnsStoredEvent()
    {
        var postResponse = await _client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-1",
            eventType = "app_open",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var getResponse = await _client.GetAsync("/api/v1/internal/events?limit=5");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var json = JsonDocument.Parse(await getResponse.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        Assert.Equal("evt-test-1", items[0].GetProperty("eventId").GetString());
    }

    [Fact]
    public async Task GetEventsSummary_ReturnsCounts()
    {
        await _client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-2",
            eventType = "holding_add",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        var response = await _client.GetAsync("/api/v1/internal/events/summary?window=50");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.True(data.GetProperty("totalEvents").GetInt32() > 0);
    }
}
