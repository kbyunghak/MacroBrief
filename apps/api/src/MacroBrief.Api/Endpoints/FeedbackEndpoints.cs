public static class FeedbackEndpoints
{
    public static void MapFeedbackEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/feedback/relevance", (RelevanceFeedbackRequest request, IFeedbackService feedbackService) =>
        {
            var normalizedSymbol = (request.Symbol ?? string.Empty).Trim().ToUpperInvariant();
            var normalizedFeedback = (request.Feedback ?? string.Empty).Trim().ToLowerInvariant();
            var newsEventId = (request.NewsEventId ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(newsEventId) || string.IsNullOrWhiteSpace(normalizedSymbol))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_request", "news_event_id and symbol are required."));
            }

            if (normalizedFeedback is not ("relevant" or "not_relevant"))
            {
                return Results.BadRequest(ApiResponse<object>.Fail("invalid_feedback", "feedback must be relevant or not_relevant."));
            }

            feedbackService.Add(new RelevanceFeedbackItem(newsEventId, normalizedSymbol, normalizedFeedback, DateTime.UtcNow));
            return Results.Created("/api/v1/feedback/relevance", ApiResponse<object>.Ok(new { accepted = true }));
        });
    }
}
