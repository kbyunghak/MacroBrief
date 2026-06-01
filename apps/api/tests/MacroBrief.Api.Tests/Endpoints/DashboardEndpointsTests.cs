using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class DashboardEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DashboardEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSummary_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/dashboard/summary");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
