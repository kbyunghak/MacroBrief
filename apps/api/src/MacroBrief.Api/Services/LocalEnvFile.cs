public static class LocalEnvFile
{
    public static void LoadFromWorkingDirectory()
    {
        if (Environment.GetEnvironmentVariable("MB_SKIP_LOCAL_ENV")?.Equals("true", StringComparison.OrdinalIgnoreCase) is true)
        {
            return;
        }

        foreach (var path in GetCandidatePaths())
        {
            Load(path);
        }
    }

    private static IEnumerable<string> GetCandidatePaths()
    {
        var current = Directory.GetCurrentDirectory();
        yield return Path.Combine(current, ".env.local");
        yield return Path.Combine(current, "apps", "api", ".env.local");

        var directory = new DirectoryInfo(current);
        while (directory is not null)
        {
            yield return Path.Combine(directory.FullName, "apps", "api", ".env.local");
            directory = directory.Parent;
        }
    }

    private static void Load(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        foreach (var line in File.ReadAllLines(path))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = trimmed.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = trimmed[..separatorIndex].Trim();
            var value = trimmed[(separatorIndex + 1)..].Trim().Trim('"');

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
