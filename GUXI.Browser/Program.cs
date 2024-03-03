using System.Threading.Tasks;
using System.Runtime.Versioning;

using Avalonia;
using Avalonia.Media;
using Avalonia.Browser;

using GUXI;

[assembly:SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(string[] args) =>
        await BuildAvaloniaApp()
            .With(new FontManagerOptions() { DefaultFamilyName = "avares://GUXI/Assets/Fonts#Roboto" })
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>();
}
