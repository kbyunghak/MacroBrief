using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class SystemEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SystemEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetStorageStatus_ReturnsConfiguredMode()
    {
        var response = await _client.GetAsync("/api/v1/internal/storage");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal("memory", data.GetProperty("mode").GetString());
        Assert.True(data.TryGetProperty("localDataDirectoryExists", out _));
    }

    [Fact]
    public async Task GetLocalDataExport_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/internal/local-data/export");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal("memory", data.GetProperty("mode").GetString());
        Assert.True(data.TryGetProperty("files", out _));
    }

    [Fact]
    public async Task PostLocalDataReset_ReturnsOk()
    {
        var response = await _client.PostAsync("/api/v1/internal/local-data/reset", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal("memory", data.GetProperty("mode").GetString());
        Assert.Equal(0, data.GetProperty("deletedFileCount").GetInt32());
    }
}
