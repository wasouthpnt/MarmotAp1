
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
namespace MarmotAp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Initialise the toolkit
        builder.UseMauiApp<App>()
            .ConfigureSyncfusionCore().UseMauiCommunityToolkit();

        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()

            .ConfigureSyncfusionToolkit()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa_solid.ttf", "FontAwesome");
                fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
            });


        builder.Services.AddSingleton<BluetoothLEService>();

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<HomePage>();

        builder.Services.AddSingleton<StatusPageViewModel>();
        builder.Services.AddSingleton<StatusPage>();

        builder.Services.AddSingleton<SettingsPageViewModel>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddSingleton<InstructionsPageViewModel>();
        builder.Services.AddSingleton<InstructionsPage>();

        builder.Services.AddSingleton<AboutPageViewModel>();
        builder.Services.AddSingleton<AboutPage>();

        builder.Services.AddSingleton<PrivacyStatementPageViewModel>();
        builder.Services.AddSingleton<PrivacyStatementPage>();


#if DEBUG
        //builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
