public record LocalJsonStorageOptions(string DataDirectory)
{
    public static LocalJsonStorageOptions FromConfiguration(IConfiguration configuration, IHostEnvironment environment)
    {
        var configuredDirectory = configuration["MB_LOCAL_DATA_DIR"];
        var repoRoot = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "..", ".."));
        var dataDirectory = string.IsNullOrWhiteSpace(configuredDirectory)
            ? Path.Combine(repoRoot, ".local-data")
            : configuredDirectory;

        if (!Path.IsPathRooted(dataDirectory))
        {
            dataDirectory = Path.Combine(repoRoot, dataDirectory);
        }

        return new LocalJsonStorageOptions(Path.GetFullPath(dataDirectory));
    }
}
