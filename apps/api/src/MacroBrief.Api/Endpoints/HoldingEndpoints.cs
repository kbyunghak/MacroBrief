public static class HoldingEndpoints
{
    public static void MapHoldingEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/holdings", (IHoldingsService holdingsService) =>
            Results.Ok(ApiResponse<IEnumerable<Holding>>.Ok(holdingsService.GetAll())));

        app.MapPost("/api/v1/holdings", (AddHoldingRequest request, IHoldingsService holdingsService) =>
        {
            if (!holdingsService.TryAdd(request.Symbol, out var holding, out var errorCode, out var errorMessage))
            {
                var error = ApiResponse<object>.Fail(errorCode ?? "invalid_request", errorMessage ?? "Invalid request.");
                return errorCode == "duplicate_symbol" ? Results.Conflict(error) : Results.BadRequest(error);
            }

            return Results.Created($"/api/v1/holdings/{holding!.Symbol}", ApiResponse<Holding>.Ok(holding));
        });

        app.MapDelete("/api/v1/holdings/{symbol}", (string symbol, IHoldingsService holdingsService) =>
        {
            if (!holdingsService.TryRemove(symbol))
            {
                return Results.NotFound(ApiResponse<object>.Fail("not_found", "Holding not found."));
            }

            return Results.NoContent();
        });
    }
}
