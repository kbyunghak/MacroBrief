public record LocalDataResetResult(
    string Mode,
    string? LocalDataDirectory,
    int DeletedFileCount,
    IReadOnlyList<string> DeletedFiles);
