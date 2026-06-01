public class InMemoryHoldingsServiceTests
{
    [Fact]
    public void GetAll_ReturnsSeededSymbolsSorted()
    {
        var service = new InMemoryHoldingsService();

        var symbols = service.GetAll().Select(x => x.Symbol).ToArray();

        Assert.Equal(new[] { "NVDA", "TSLA", "XOM" }, symbols);
    }

    [Fact]
    public void TryAdd_WithValidSymbol_AddsHolding()
    {
        var service = new InMemoryHoldingsService();

        var added = service.TryAdd("sofi", out var holding, out _, out _);

        Assert.True(added);
        Assert.NotNull(holding);
        Assert.Equal("SOFI", holding!.Symbol);
    }

    [Fact]
    public void TryAdd_WithDuplicateSymbol_ReturnsDuplicateError()
    {
        var service = new InMemoryHoldingsService();

        var added = service.TryAdd("TSLA", out _, out var errorCode, out _);

        Assert.False(added);
        Assert.Equal("duplicate_symbol", errorCode);
    }
}
