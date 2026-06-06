public static class SystemEndpoints
{
    public static void MapSystemEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/internal/storage", (IStorageStatusService storageStatusService) =>
            Results.Ok(ApiResponse<StorageStatus>.Ok(storageStatusService.GetStatus())));

        app.MapGet("/api/v1/internal/local-data/export", (ILocalDataManagementService localDataManagementService) =>
            Results.Ok(ApiResponse<LocalDataExport>.Ok(localDataManagementService.ExportStatus())));

        app.MapPost("/api/v1/internal/local-data/reset", (ILocalDataManagementService localDataManagementService) =>
            Results.Ok(ApiResponse<LocalDataResetResult>.Ok(localDataManagementService.Reset())));
    }
}
