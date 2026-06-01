using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var holdings = new List<Holding>
{
    new("TSLA"),
    new("XOM"),
    new("NVDA")
};

app.MapGet("/api/v1/dashboard/summary", () =>
{
    var now = DateTime.UtcNow;
    var payload = new DashboardSummary(
        LastUpdatedAt: now,
        HoldingsCount: holdings.Count,
        RelatedUpdatesCount: 12,
        TopExposureCategories: new[] { "Interest Rates", "Oil & Geopolitics", "AI / Semiconductors" },
        MorningBrief: new[]
        {
            "Fed tone remains hawkish.",
            "10-year Treasury yields moved higher.",
            "Oil rose on geopolitical supply concerns."
        });

    return Results.Ok(ApiResponse<DashboardSummary>.Ok(payload));
});

app.MapGet("/api/v1/holdings", () =>
    Results.Ok(ApiResponse<IEnumerable<Holding>>.Ok(holdings.OrderBy(h => h.Symbol))));

app.MapPost("/api/v1/holdings", (AddHoldingRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Symbol))
    {
        return Results.BadRequest(ApiResponse<object>.Fail("invalid_symbol", "Symbol is required."));
    }

    var normalized = request.Symbol.Trim().ToUpperInvariant();
    if (!Regex.IsMatch(normalized, "^[A-Z\\.]{1,10}$"))
    {
        return Results.BadRequest(ApiResponse<object>.Fail("invalid_symbol", "Symbol format is invalid."));
    }

    if (holdings.Any(h => h.Symbol == normalized))
    {
        return Results.Conflict(ApiResponse<object>.Fail("duplicate_symbol", "Symbol already exists."));
    }

    var holding = new Holding(normalized);
    holdings.Add(holding);
    return Results.Created($"/api/v1/holdings/{holding.Symbol}", ApiResponse<Holding>.Ok(holding));
});

app.MapDelete("/api/v1/holdings/{symbol}", (string symbol) =>
{
    var normalized = symbol.Trim().ToUpperInvariant();
    var match = holdings.FirstOrDefault(h => h.Symbol == normalized);
    if (match is null)
    {
        return Results.NotFound(ApiResponse<object>.Fail("not_found", "Holding not found."));
    }

    holdings.Remove(match);
    return Results.NoContent();
});

app.Run();

record Holding(string Symbol);

record AddHoldingRequest(string Symbol);

record DashboardSummary(
    DateTime LastUpdatedAt,
    int HoldingsCount,
    int RelatedUpdatesCount,
    IEnumerable<string> TopExposureCategories,
    IEnumerable<string> MorningBrief);

record ApiResponse<T>(T? Data, ApiError? Error, ApiMeta? Meta)
{
    public static ApiResponse<T> Ok(T data) => new(data, null, new ApiMeta(DateTime.UtcNow));

    public static ApiResponse<T> Fail(string code, string message) =>
        new(default, new ApiError(code, message), new ApiMeta(DateTime.UtcNow));
}

record ApiError(string Code, string Message);

record ApiMeta(DateTime TimestampUtc);
