using System.Text.RegularExpressions;

public class InMemoryHoldingsService : IHoldingsService
{
    private readonly List<Holding> _holdings =
    [
        new("TSLA"),
        new("XOM"),
        new("NVDA")
    ];

    public IReadOnlyList<Holding> GetAll() => _holdings.OrderBy(h => h.Symbol).ToList();

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

        if (_holdings.Any(h => h.Symbol == normalized))
        {
            errorCode = "duplicate_symbol";
            errorMessage = "Symbol already exists.";
            return false;
        }

        holding = new Holding(normalized);
        _holdings.Add(holding);
        return true;
    }

    public bool TryRemove(string symbol)
    {
        var normalized = (symbol ?? string.Empty).Trim().ToUpperInvariant();
        var match = _holdings.FirstOrDefault(h => h.Symbol == normalized);
        if (match is null)
        {
            return false;
        }

        _holdings.Remove(match);
        return true;
    }
}
