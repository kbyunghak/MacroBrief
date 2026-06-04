public static class SystemEndpoints
{
    public static void MapSystemEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/internal/storage", (IStorageStatusService storageStatusService) =>
            Results.Ok(ApiResponse<StorageStatus>.Ok(storageStatusService.GetStatus())));
    }
}
