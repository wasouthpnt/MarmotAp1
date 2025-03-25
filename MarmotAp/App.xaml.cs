using System.Reflection.PortableExecutable;

namespace MarmotAp;

// App structure reference "https://github.com/TimoKinnunen/MauiXAMLBluetoothLE"

// Ported over from Marmot (C:\Users\wasou\source\repos\Marmot)
public partial class App : Application
{
    public static IDevice g_Device = null;
    public static IService g_Service = null;
    public static ICharacteristic g_Characteristic_1 = null;
    public static ICharacteristic g_Characteristic_2 = null;
    public static Packet10hz packet10Hz = null;

    public App()
	{
		InitializeComponent();
        
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc2OTgzOEAzMjM4MmUzMDJlMzBCNWxoYjE3ZEpMWnEwb0ZOSnFwUDJhWXVQNUk5YVlmeVpGaTUzMVdhZlp3PQ==");
        string s = Preferences.Get("@string/DevKey", "");
        Preferences.Set("MPH", "0");
        if (String.IsNullOrEmpty(s))
        {
            Preferences.Set("@string/DevKey", "_Anteater");
        }

        // **************** Remove after Settings page is developed
        Preferences.Set("@string/CmdHeader", "was_");


        MainPage = new AppShell();
	}

	protected override void OnSleep()
	{
		base.OnSleep();
	}

	protected override void OnResume()
	{
		base.OnResume();
	}
}
