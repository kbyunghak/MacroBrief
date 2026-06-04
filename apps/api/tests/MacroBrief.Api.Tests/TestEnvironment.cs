using System.Runtime.CompilerServices;

public static class TestEnvironment
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Environment.SetEnvironmentVariable("MB_SKIP_LOCAL_ENV", "true");
        Environment.SetEnvironmentVariable("MB_STORAGE_MODE", "memory");
    }
}
