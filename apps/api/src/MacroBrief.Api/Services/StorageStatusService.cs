public class StorageStatusService : IStorageStatusService
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public StorageStatusService(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public StorageStatus GetStatus()
    {
        var mode = _configuration["MB_STORAGE_MODE"] ?? "memory";
        if (!mode.Equals("local_json", StringComparison.OrdinalIgnoreCase))
        {
            return new StorageStatus("memory", null, false);
        }

        var options = LocalJsonStorageOptions.FromConfiguration(_configuration, _environment);
        return new StorageStatus(
            "local_json",
            options.DataDirectory,
            Directory.Exists(options.DataDirectory));
    }
}
