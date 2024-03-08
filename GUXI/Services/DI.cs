using System;
using System.Linq;

using Shared.Services;

using Microsoft.Extensions.DependencyInjection;

namespace GUXI.Services
{
public static class DI
{
    public static ServiceProvider ServiceProvider { get; set; }

    private static ServiceCollection Services { get; set; } = new();

    static DI()
    {
        Services.AddSingleton<ClipboardService>()
            .AddSingleton<SharedService>()
            .AddSingleton<NotificationService>()
            .AddSingleton<StorageService>()
            .AddSingleton<SettingsService>();

        ServiceProvider = Services.BuildServiceProvider();
    }

    public static Type Create<Type>(params object[] parameters) =>
        ActivatorUtilities.CreateInstance<Type>(ServiceProvider, parameters);

    public static object Create(Type type,
                                params object[] parameters) => ActivatorUtilities.CreateInstance(ServiceProvider, type,
                                                                                                 parameters);

    public static void Initialize() => Apply(service => service.Initialize());
    public static void Uninitialize() => Apply(service => service.Uninitialize());

    private static void Apply(Action<ServiceBase> action)
    {
        Services
            .Where(serviceDescriptor => serviceDescriptor.ImplementationType != null &&
                                        serviceDescriptor.ImplementationType.IsSubclassOf(typeof(ServiceBase)))
            .Select(serviceDescriptor => ServiceProvider.GetService(serviceDescriptor.ImplementationType!))
            .Where(obj => obj is not null)
            .Select(obj => (ServiceBase)obj!)
            .ToList()
            .ForEach(action);
    }
}
}
