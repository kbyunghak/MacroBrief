using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

public class LocalDataManagementServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void ExportStatus_WithMemoryMode_ReturnsNoFiles()
    {
        var service = new LocalDataManagementService(
            BuildConfiguration(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "memory"
            }),
            new StubHostEnvironment());

        var export = service.ExportStatus();

        Assert.Equal("memory", export.Mode);
        Assert.Null(export.LocalDataDirectory);
        Assert.Empty(export.Files);
    }

    [Fact]
    public void ExportStatus_WithLocalJsonMode_ReturnsManagedFiles()
    {
        Directory.CreateDirectory(_dataDirectory);
        File.WriteAllText(Path.Combine(_dataDirectory, "holdings.json"), "[]");
        File.WriteAllText(Path.Combine(_dataDirectory, "ignored.txt"), "not-managed");
        var service = BuildLocalJsonService();

        var export = service.ExportStatus();

        Assert.Equal("local_json", export.Mode);
        Assert.Equal(_dataDirectory, export.LocalDataDirectory);
        Assert.Single(export.Files);
        Assert.Equal("holdings.json", export.Files[0].Name);
    }

    [Fact]
    public void Reset_WithLocalJsonMode_DeletesOnlyManagedFiles()
    {
        Directory.CreateDirectory(_dataDirectory);
        File.WriteAllText(Path.Combine(_dataDirectory, "holdings.json"), "[]");
        File.WriteAllText(Path.Combine(_dataDirectory, "kpi-events.json"), "[]");
        File.WriteAllText(Path.Combine(_dataDirectory, "ignored.txt"), "not-managed");
        var service = BuildLocalJsonService();

        var reset = service.Reset();

        Assert.Equal("local_json", reset.Mode);
        Assert.Equal(2, reset.DeletedFileCount);
        Assert.Contains("holdings.json", reset.DeletedFiles);
        Assert.Contains("kpi-events.json", reset.DeletedFiles);
        Assert.False(File.Exists(Path.Combine(_dataDirectory, "holdings.json")));
        Assert.False(File.Exists(Path.Combine(_dataDirectory, "kpi-events.json")));
        Assert.True(File.Exists(Path.Combine(_dataDirectory, "ignored.txt")));
    }

    private LocalDataManagementService BuildLocalJsonService()
    {
        return new LocalDataManagementService(
            BuildConfiguration(new Dictionary<string, string?>
            {
                ["MB_STORAGE_MODE"] = "local_json",
                ["MB_LOCAL_DATA_DIR"] = _dataDirectory
            }),
            new StubHostEnvironment());
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
