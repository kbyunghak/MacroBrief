public class LocalJsonHoldingsServiceTests : IDisposable
{
    private readonly string _dataDirectory = Path.Combine(Path.GetTempPath(), $"macrobrief-test-{Guid.NewGuid():N}");

    [Fact]
    public void GetAll_CreatesSeededHoldingsFile_WhenMissing()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var service = new LocalJsonHoldingsService(options);

        var symbols = service.GetAll().Select(x => x.Symbol).ToArray();

        Assert.Equal(new[] { "NVDA", "TSLA", "XOM" }, symbols);
        Assert.True(File.Exists(Path.Combine(_dataDirectory, "holdings.json")));
    }

    [Fact]
    public void TryAdd_PersistsHoldingAcrossServiceInstances()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonHoldingsService(options);

        var added = firstService.TryAdd("sofi", out var holding, out _, out _);

        var secondService = new LocalJsonHoldingsService(options);
        var symbols = secondService.GetAll().Select(x => x.Symbol).ToArray();

        Assert.True(added);
        Assert.NotNull(holding);
        Assert.Contains("SOFI", symbols);
    }

    [Fact]
    public void TryAdd_WithDuplicateAcrossServiceInstances_ReturnsDuplicateError()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonHoldingsService(options);
        Assert.True(firstService.TryAdd("sofi", out _, out _, out _));

        var secondService = new LocalJsonHoldingsService(options);
        var addedAgain = secondService.TryAdd("SOFI", out _, out var errorCode, out _);

        Assert.False(addedAgain);
        Assert.Equal("duplicate_symbol", errorCode);
    }

    [Fact]
    public void TryRemove_PersistsRemovalAcrossServiceInstances()
    {
        var options = new LocalJsonStorageOptions(_dataDirectory);
        var firstService = new LocalJsonHoldingsService(options);
        Assert.True(firstService.TryAdd("sofi", out _, out _, out _));

        var removed = firstService.TryRemove("SOFI");

        var secondService = new LocalJsonHoldingsService(options);
        var symbols = secondService.GetAll().Select(x => x.Symbol).ToArray();

        Assert.True(removed);
        Assert.DoesNotContain("SOFI", symbols);
    }

    public void Dispose()
    {
        if (Directory.Exists(_dataDirectory))
        {
            Directory.Delete(_dataDirectory, recursive: true);
        }
    }
}
