using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class KpiEventsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public KpiEventsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IKpiEventService>();
                services.AddSingleton<IKpiEventService, InMemoryKpiEventService>();
            });
        }).CreateClient();
    }

    [Fact]
    public async Task PostEvent_ThenGetRecent_ReturnsStoredEvent()
    {
        var client = CreateClient();

        var postResponse = await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-1",
            eventType = "app_open",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var getResponse = await client.GetAsync("/api/v1/internal/events?limit=5");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var json = JsonDocument.Parse(await getResponse.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        Assert.Equal("evt-test-1", items[0].GetProperty("eventId").GetString());
    }

    [Fact]
    public async Task GetEventsSummary_ReturnsCounts()
    {
        var client = CreateClient();

        await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-2",
            eventType = "holding_add",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-3",
            eventType = "holding_add",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-test-4",
            eventType = "source_click",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        var response = await client.GetAsync("/api/v1/internal/events/summary?window=50");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        var counts = data.GetProperty("eventTypeCounts");
        Assert.Equal(3, data.GetProperty("totalEvents").GetInt32());
        Assert.Equal(2, counts.GetProperty("holding_add").GetInt32());
        Assert.Equal(1, counts.GetProperty("source_click").GetInt32());
    }

    [Fact]
    public async Task PostEvent_WithInvalidEventType_ReturnsBadRequest()
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-invalid-type",
            eventType = "portfolio_prediction",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var error = json.RootElement.GetProperty("error");
        Assert.Equal("invalid_event_type", error.GetProperty("code").GetString());
    }

    [Fact]
    public async Task PostEvent_WithMissingRequiredField_ReturnsBadRequest()
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-missing-field",
            eventType = "app_open",
            userId = "",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var error = json.RootElement.GetProperty("error");
        Assert.Equal("invalid_request", error.GetProperty("code").GetString());
    }

    [Fact]
    public async Task GetWeeklyRollup_ReturnsKpiFields()
    {
        var client = CreateClient();

        await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-rollup-fields",
            eventType = "impact_feedback",
            userId = "usr-test",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-test",
            feedback = "relevant"
        });

        var response = await client.GetAsync("/api/v1/internal/events/weekly-rollup?days=7");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.True(data.GetProperty("weeklyActiveUsers").GetInt32() >= 1);
        Assert.Equal(1, data.GetProperty("feedbackSampleSize").GetInt32());
        Assert.True(data.GetProperty("relevancePositiveRatio").GetDouble() >= 0);
        Assert.False(string.IsNullOrWhiteSpace(data.GetProperty("kpiHealth").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(data.GetProperty("recommendation").GetString()));
    }

    [Fact]
    public async Task GetWeeklyRollup_WithSmallFeedbackSample_ReturnsInsufficientData()
    {
        var client = CreateClient();

        await client.PostAsJsonAsync("/api/v1/events", new
        {
            eventId = "evt-small-sample-1",
            eventType = "impact_feedback",
            userId = "usr-small-sample",
            occurredAtUtc = DateTime.UtcNow,
            sessionId = "ses-small-sample",
            feedback = "relevant"
        });

        var response = await client.GetAsync("/api/v1/internal/events/weekly-rollup?days=7");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(1, data.GetProperty("feedbackSampleSize").GetInt32());
        Assert.Equal("insufficient_data", data.GetProperty("kpiHealth").GetString());
        Assert.Equal("collect_more_data", data.GetProperty("recommendation").GetString());
    }

    [Fact]
    public async Task GetWeeklyRollup_WithEnoughPositiveFeedback_ReturnsGreenProceed()
    {
        var client = CreateClient();

        for (var i = 0; i < 5; i++)
        {
            await client.PostAsJsonAsync("/api/v1/events", new
            {
                eventId = $"evt-green-{i}",
                eventType = "impact_feedback",
                userId = $"usr-green-{i}",
                occurredAtUtc = DateTime.UtcNow,
                sessionId = $"ses-green-{i}",
                feedback = i < 4 ? "relevant" : "not_relevant"
            });
        }

        var response = await client.GetAsync("/api/v1/internal/events/weekly-rollup?days=7");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(5, data.GetProperty("feedbackSampleSize").GetInt32());
        Assert.Equal(0.8, data.GetProperty("relevancePositiveRatio").GetDouble());
        Assert.Equal("green", data.GetProperty("kpiHealth").GetString());
        Assert.Equal("proceed", data.GetProperty("recommendation").GetString());
    }

    [Fact]
    public async Task GetWeeklyRollup_WithMixedFeedback_ReturnsYellowIterate()
    {
        var client = CreateClient();

        for (var i = 0; i < 5; i++)
        {
            await client.PostAsJsonAsync("/api/v1/events", new
            {
                eventId = $"evt-yellow-{i}",
                eventType = "impact_feedback",
                userId = $"usr-yellow-{i}",
                occurredAtUtc = DateTime.UtcNow,
                sessionId = $"ses-yellow-{i}",
                feedback = i < 3 ? "relevant" : "not_relevant"
            });
        }

        var response = await client.GetAsync("/api/v1/internal/events/weekly-rollup?days=7");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(5, data.GetProperty("feedbackSampleSize").GetInt32());
        Assert.Equal(0.6, data.GetProperty("relevancePositiveRatio").GetDouble());
        Assert.Equal("yellow", data.GetProperty("kpiHealth").GetString());
        Assert.Equal("iterate", data.GetProperty("recommendation").GetString());
    }

    [Fact]
    public async Task GetWeeklyRollup_WithLowPositiveFeedback_ReturnsRedReposition()
    {
        var client = CreateClient();

        for (var i = 0; i < 5; i++)
        {
            await client.PostAsJsonAsync("/api/v1/events", new
            {
                eventId = $"evt-red-{i}",
                eventType = "impact_feedback",
                userId = $"usr-red-{i}",
                occurredAtUtc = DateTime.UtcNow,
                sessionId = $"ses-red-{i}",
                feedback = i < 2 ? "relevant" : "not_relevant"
            });
        }

        var response = await client.GetAsync("/api/v1/internal/events/weekly-rollup?days=7");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(5, data.GetProperty("feedbackSampleSize").GetInt32());
        Assert.Equal(0.4, data.GetProperty("relevancePositiveRatio").GetDouble());
        Assert.Equal("red", data.GetProperty("kpiHealth").GetString());
        Assert.Equal("reposition", data.GetProperty("recommendation").GetString());
    }
}
