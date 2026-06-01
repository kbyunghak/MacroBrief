using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class FeedbackEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FeedbackEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostRelevanceFeedback_WithValidPayload_ReturnsCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/feedback/relevance", new
        {
            newsEventId = "news-1",
            symbol = "nvda",
            feedback = "relevant"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostRelevanceFeedback_WithInvalidFeedback_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/feedback/relevance", new
        {
            newsEventId = "news-1",
            symbol = "NVDA",
            feedback = "wrong_value"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
