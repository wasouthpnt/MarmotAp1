namespace MarmotAp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(StatusPage), typeof(StatusPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(InstructionsPage), typeof(InstructionsPage));
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        Routing.RegisterRoute(nameof(PrivacyStatementPage), typeof(PrivacyStatementPage));
        //Routing.RegisterRoute(nameof(BatteryLevelPage), typeof(BatteryLevelPage));
    }
}
