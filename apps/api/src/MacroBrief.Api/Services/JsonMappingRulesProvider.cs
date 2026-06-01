using System.Text.Json;

public class JsonMappingRulesProvider : IMappingRulesProvider
{
    private readonly Dictionary<string, List<string>> _exposureByTicker;

    public JsonMappingRulesProvider(IHostEnvironment environment)
    {
        var root = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "..", ".."));
        var path = Path.Combine(root, "docs", "mapping_rules.v1.json");
        if (!File.Exists(path))
        {
            _exposureByTicker = new(StringComparer.OrdinalIgnoreCase);
            return;
        }

        var json = File.ReadAllText(path);
        var doc = JsonDocument.Parse(json);
        var map = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (doc.RootElement.TryGetProperty("ticker_exposure", out var tickerExposure))
        {
            foreach (var prop in tickerExposure.EnumerateObject())
            {
                map[prop.Name] = prop.Value.EnumerateArray().Select(x => x.GetString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }
        }

        _exposureByTicker = map;
    }

    public IReadOnlyList<string> GetExposureTags(string symbol)
    {
        return _exposureByTicker.TryGetValue(symbol, out var tags) ? tags : [];
    }
}
