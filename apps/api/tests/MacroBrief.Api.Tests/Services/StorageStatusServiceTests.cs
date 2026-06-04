using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class StorageStatusServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void GetStatus_WithMemoryMode_DoesNotExposeLocalDataDirectory()
    {
        var service = new StorageStatusService(
            BuildConfiguration(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "memory"
            }),
            new StubHostEnvironment());

        var status = service.GetStatus();

        Assert.Equal("memory", status.Mode);
        Assert.Null(status.LocalDataDirectory);
        Assert.False(status.LocalDataDirectoryExists);
    }

    [Fact]
    public void GetStatus_WithLocalJsonMode_ReturnsResolvedDataDirectory()
    {
        Directory.CreateDirectory(_dataDirectory);
        var service = new StorageStatusService(
            BuildConfiguration(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "local_json",
                ["MB_LOCAL_DATA_DIR"] = _dataDirectory
            }),
            new StubHostEnvironment());

        var status = service.GetStatus();

        Assert.Equal("local_json", status.Mode);
        Assert.Equal(_dataDirectory, status.LocalDataDirectory);
        Assert.True(status.LocalDataDirectoryExists);
    }

    private static IConfiguration BuildConfiguration(IReadOnlyDictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    public void Dispose()
    {
        if (Directory.Exists(_dataDirectory))
        {
            Directory.Delete(_dataDirectory, recursive: true);
        }
    }

    private sealed class StubHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; } = "MacroBrief.Api.Tests";
        public string ContentRootPath { get; set; } = Path.Combine(
            Directory.GetCurrentDirectory(),
            "apps",
            "api",
            "src",
            "MacroBrief.Api");
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
