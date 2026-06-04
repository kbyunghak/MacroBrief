using System.Text.RegularExpressions;

public class LocalJsonHoldingsService : IHoldingsService
{
    private readonly JsonFileStore<Holding> _store;

    public LocalJsonHoldingsService(LocalJsonStorageOptions options)
    {
        _store = new JsonFileStore<Holding>(
            Path.Combine(options.DataDirectory, "holdings.json"),
            () => [new("TSLA"), new("XOM"), new("NVDA")]);
    }

    public IReadOnlyList<Holding> GetAll() => _store.ReadAll().OrderBy(h => h.Symbol).ToList();

    public bool TryAdd(string symbol, out Holding? holding, out string? errorCode, out string? errorMessage)
    {
        holding = null;
        errorCode = null;
        errorMessage = null;

        var normalized = (symbol ?? string.Empty).Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            errorCode = "invalid_symbol";
            errorMessage = "Symbol is required.";
            return false;
        }

        if (!Regex.IsMatch(normalized, "^[A-Z\\.]{1,10}$"))
        {
            errorCode = "invalid_symbol";
            errorMessage = "Symbol format is invalid.";
            return false;
        }

        var addedHolding = new Holding(normalized);
        var added = false;
        _store.Update(current =>
        {
            if (current.Any(h => h.Symbol == normalized))
            {
                return current;
            }

            current.Add(addedHolding);
            added = true;
            return current;
        });

        if (!added)
        {
            errorCode = "duplicate_symbol";
            errorMessage = "Symbol already exists.";
            return false;
        }

        holding = addedHolding;
        return true;
    }

    public bool TryRemove(string symbol)
    {
        var normalized = (symbol ?? string.Empty).Trim().ToUpperInvariant();
        var removed = false;
        _store.Update(current =>
        {
            var match = current.FirstOrDefault(h => h.Symbol == normalized);
            if (match is null)
            {
                return current;
            }

            current.Remove(match);
            removed = true;
            return current;
        });

        return removed;
    }
}
