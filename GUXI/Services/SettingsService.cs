using Shared.Services;

namespace GUXI.Services
{
public class SettingsService : ServiceBase
{
    private readonly StorageService _storage;

    public SettingsService(StorageService storage)
    {
        _storage = storage;
    }
}
}
