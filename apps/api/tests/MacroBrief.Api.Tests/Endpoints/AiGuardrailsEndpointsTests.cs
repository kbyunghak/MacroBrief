using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class AiGuardrailsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AiGuardrailsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAuditLogs_ReturnsGeneratedLogs()
    {
        var cardsResponse = await _client.GetAsync("/api/v1/impact-cards");
        Assert.Equal(HttpStatusCode.OK, cardsResponse.StatusCode);

        var auditResponse = await _client.GetAsync("/api/v1/internal/ai/audit?limit=5");
        Assert.Equal(HttpStatusCode.OK, auditResponse.StatusCode);

        var json = JsonDocument.Parse(await auditResponse.Content.ReadAsStringAsync());
        var items = json.RootElement.GetProperty("data");
        Assert.True(items.GetArrayLength() > 0);
        Assert.False(string.IsNullOrWhiteSpace(items[0].GetProperty("promptVersion").GetString()));
    }
}
