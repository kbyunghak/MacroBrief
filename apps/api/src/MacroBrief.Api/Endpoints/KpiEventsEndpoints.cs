public static class KpiEventsEndpoints
{
    private static readonly HashSet<string> AllowedEventTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "app_open",
        "dashboard_refresh",
        "holding_add",
        "holding_remove",
        "impact_feedback",
        "alert_view",
        "source_click"
    };

    public static void MapKpiEventsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/events", (KpiEventRequest request, IKpiEventService service) =>
        {
            if (string.IsNullOrWhiteSpace(request.EventId) ||
                string.IsNullOrWhiteSpace(request.EventType) ||
                string.IsNullOrWhiteSpace(request.UserId) ||
                string.IsNullOrWhiteSpace(request.SessionId))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_request", "event_id, event_type, user_id, and session_id are required."));
            }

            if (!AllowedEventTypes.Contains(request.EventType))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_event_type", "Unsupported event_type."));
            }

            var item = new KpiEventItem(
                EventId: request.EventId.Trim(),
                EventType: request.EventType.Trim(),
                UserId: request.UserId.Trim(),
                OccurredAtUtc: request.OccurredAtUtc,
                SessionId: request.SessionId.Trim(),
                Symbol: request.Symbol?.Trim().ToUpperInvariant(),
                NewsEventId: request.NewsEventId?.Trim(),
                Feedback: request.Feedback?.Trim().ToLowerInvariant(),
                ReasonTag: request.ReasonTag?.Trim(),
                SourceName: request.SourceName?.Trim(),
                SourceUrl: request.SourceUrl?.Trim(),
                Meta: request.Meta,
                IngestedAtUtc: DateTime.UtcNow);

            service.Add(item);
            return Results.Created("/api/v1/events", ApiResponse<object>.Ok(new { accepted = true }));
        });

        app.MapGet("/api/v1/internal/events", (int? limit, IKpiEventService service) =>
        {
            var items = service.GetRecent(limit ?? 100);
            return Results.Ok(ApiResponse<IEnumerable<KpiEventItem>>.Ok(items));
        });
    }
}
