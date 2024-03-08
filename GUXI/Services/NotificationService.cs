using System;
using System.Collections.ObjectModel;

using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.ApplicationLifetimes;

using Shared.Services;

namespace GUXI.Services
{
public class NotificationService : ServiceBase
{
    private WindowNotificationManager? _manager = null;

    private readonly ObservableCollection<INotification> _cache = [];

    public override void Initialize()
    {
        if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _manager = new(desktop.MainWindow);
        }
        else if (Avalonia.Application.Current!.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            _manager = new(TopLevel.GetTopLevel(singleViewPlatform.MainView));
        }
        else
        {
            throw new NotSupportedException("Unsupported platform");
        }

        _manager.Position = NotificationPosition.BottomRight;
        _manager.MaxItems = 2;

        _manager.TemplateApplied += (sender, args) =>
        {
            if (_cache == null)
            {
                return;
            }

            foreach (var notification in _cache)
            {
                _manager!.Show(notification);
            }

            _cache.Clear();
        };
    }

    private void Notify(Notification notification)
    {
        if (_cache.Count > 0)
        {
            _cache.Add(notification);
        }
        else
        {
            _manager!.Show(notification);
        }
    }

    public void NotifyI(string message) => Notify(new("GUXI", message, NotificationType.Information));
    public void NotifyE(string message) => Notify(new("GUXI", message, NotificationType.Error));
}
}
