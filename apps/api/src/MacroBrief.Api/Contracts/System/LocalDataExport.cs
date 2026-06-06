public record LocalDataExport(
    string Mode,
    string? LocalDataDirectory,
    IReadOnlyList<LocalDataFileStatus> Files);
