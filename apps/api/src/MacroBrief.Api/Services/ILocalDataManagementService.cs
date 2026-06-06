public interface ILocalDataManagementService
{
    LocalDataExport ExportStatus();
    LocalDataResetResult Reset();
}
