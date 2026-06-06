public class LocalDataManagementService : ILocalDataManagementService
{
    private static readonly string[] ManagedFileNames =
    [
        "holdings.json",
        "relevance-feedback.json",
        "kpi-events.json",
        "ai-explanation-audit.json"
    ];

    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public LocalDataManagementService(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public LocalDataExport ExportStatus()
    {
        var mode = GetMode();
        if (!mode.Equals("local_json", StringComparison.OrdinalIgnoreCase))
        {
            return new LocalDataExport("memory", null, []);
        }

        var options = LocalJsonStorageOptions.FromConfiguration(_configuration, _environment);
        var files = ManagedFileNames
            .Select(name => new FileInfo(Path.Combine(options.DataDirectory, name)))
            .Where(file => file.Exists)
            .Select(file => new LocalDataFileStatus(
                file.Name,
                file.Length,
                file.LastWriteTimeUtc))
            .OrderBy(file => file.Name)
            .ToList();

        return new LocalDataExport("local_json", options.DataDirectory, files);
    }

    public LocalDataResetResult Reset()
    {
        var mode = GetMode();
        if (!mode.Equals("local_json", StringComparison.OrdinalIgnoreCase))
        {
            return new LocalDataResetResult("memory", null, 0, []);
        }

        var options = LocalJsonStorageOptions.FromConfiguration(_configuration, _environment);
        var deletedFiles = new List<string>();

        foreach (var fileName in ManagedFileNames)
        {
            var path = Path.Combine(options.DataDirectory, fileName);
            if (!File.Exists(path))
            {
                continue;
            }

            File.Delete(path);
            deletedFiles.Add(fileName);
        }

        return new LocalDataResetResult("local_json", options.DataDirectory, deletedFiles.Count, deletedFiles);
    }

    private string GetMode()
    {
        return _configuration["MB_STORAGE_MODE"] ?? "memory";
    }
}
