using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Controls.ApplicationLifetimes;

using Shared.Services;

namespace GUXI.Services
{
public class ClipboardService : ServiceBase
{
    private IClipboard? _clipboard = null;

    public override void Initialize()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _clipboard = desktop.MainWindow?.Clipboard;
        }
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            _clipboard = TopLevel.GetTopLevel(singleViewPlatform.MainView)?.Clipboard;
        }
        else
        {
            throw new NotSupportedException("Unsupported platform");
        }
    }

    public async Task<string?> GetText()
    {
        if (_clipboard == null)
        {
            return null;
        }

        return await _clipboard.GetTextAsync();
    }

    public async Task SetText(string? text)
    {
        if (_clipboard == null)
        {
            return;
        }

        await _clipboard.SetTextAsync(text);
    }
}
}
