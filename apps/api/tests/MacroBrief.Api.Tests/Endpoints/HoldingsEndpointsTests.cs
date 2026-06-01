using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class HoldingsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HoldingsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHoldings_ReturnsOkEnvelope()
    {
        var response = await _client.GetAsync("/api/v1/holdings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostHolding_WithDuplicate_ReturnsConflict()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/holdings", new { symbol = "TSLA" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task DeleteHolding_RemovesSymbol()
    {
        var addResponse = await _client.PostAsJsonAsync("/api/v1/holdings", new { symbol = "SOFI" });
        Assert.Equal(HttpStatusCode.Created, addResponse.StatusCode);

        var deleteResponse = await _client.DeleteAsync("/api/v1/holdings/SOFI");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
