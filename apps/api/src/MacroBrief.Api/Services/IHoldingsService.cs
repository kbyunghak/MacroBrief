public interface IHoldingsService
{
    IReadOnlyList<Holding> GetAll();
    bool TryAdd(string symbol, out Holding? holding, out string? errorCode, out string? errorMessage);
    bool TryRemove(string symbol);
}
