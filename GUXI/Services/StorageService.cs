using System;
using System.IO;

using Shared.Services;
using Shared.Utilities;

namespace GUXI.Services
{
public class StorageService : ServiceBase
{
    private string AppDataDirectory = "";
    private string AppDataGUXIDirectory = "";

    public StorageService()
    {
        FindOrCreateDirectories();
    }

    private void FindOrCreateDirectories()
    {
        // TODO: fix this ervice for browser: https://github.com/AvaloniaUI/Avalonia/discussions/14119
        AppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        AppDataGUXIDirectory = Path.Combine(AppDataDirectory, "GUXI");
        if (!Directory.Exists(AppDataGUXIDirectory))
        {
            Directory.CreateDirectory(AppDataGUXIDirectory);
        }
    }

    public T? Load<T>(string key)
        where T : class
    {
        var path = Path.Combine(AppDataGUXIDirectory, key);
        if (!File.Exists(path))
        {
            return null;
        }

        var json = File.ReadAllBytes(path);
        return json.FromMsgPack<T>();
    }

    public void Save(string key, object obj)
    {
        var path = Path.Combine(AppDataGUXIDirectory, key);
        File.WriteAllBytes(path, obj.ToMsgPack());
    }
}
}
