using System.Text.Json;

public class JsonFileStore<T>
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly object _sync = new();
    private readonly string _path;
    private readonly Func<IReadOnlyList<T>> _seedFactory;

    public JsonFileStore(string path, Func<IReadOnlyList<T>>? seedFactory = null)
    {
        _path = path;
        _seedFactory = seedFactory ?? (() => []);
    }

    public IReadOnlyList<T> ReadAll()
    {
        lock (_sync)
        {
            EnsureFile();
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<T>>(json, SerializerOptions) ?? [];
        }
    }

    public void WriteAll(IReadOnlyList<T> items)
    {
        lock (_sync)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            var json = JsonSerializer.Serialize(items, SerializerOptions);
            var tempPath = $"{_path}.tmp";
            File.WriteAllText(tempPath, json);
            File.Move(tempPath, _path, overwrite: true);
        }
    }

    public void Update(Func<List<T>, List<T>> update)
    {
        lock (_sync)
        {
            EnsureFile();
            var json = File.ReadAllText(_path);
            var current = JsonSerializer.Deserialize<List<T>>(json, SerializerOptions) ?? [];
            WriteAll(update(current));
        }
    }

    private void EnsureFile()
    {
        if (File.Exists(_path))
        {
            return;
        }

        WriteAll(_seedFactory());
    }
}
