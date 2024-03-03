using System.Threading.Tasks;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;

using FluentAvalonia.UI.Controls;

namespace GUXI.Views.Dialogs
{
public abstract class DialogBase
{
    private readonly TaskDialog _dialog = new();
    protected IList<TaskDialogButton> Buttons => _dialog.Buttons;

    protected DialogBase(Visual? root, string header, string? subHeader = null)
    {
        _dialog.XamlRoot = root;

        _dialog.Header = header;
        _dialog.SubHeader = subHeader;
        _dialog.Content = CreateContent();
    }

    protected abstract Control CreateContent();

    protected async Task<object> Show()
    {
        return await _dialog.ShowAsync(true);
    }
}
}
