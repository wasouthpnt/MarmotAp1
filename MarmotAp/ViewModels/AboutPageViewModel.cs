using System.Text;

namespace MarmotAp.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    public BluetoothLEService BluetoothLEService { get; private set; }

    public AboutPageViewModel(BluetoothLEService bluetoothLEService)
    {
        Title = $"About MarmotAp";

        BluetoothLEService = bluetoothLEService;

        Name = AppInfo.Current.Name;
        Version = AppInfo.Current.VersionString;
        Build = AppInfo.Current.BuildString;

    }

    [ObservableProperty]
    string name;

    [ObservableProperty]
    string version;

    [ObservableProperty]
    string build;

    [ObservableProperty]
    string eSP32Ver;

    public async Task ReadESP32Ver()
    {
        try
        {
            if (App.g_Characteristic_2 != null)
            {
                string s = Preferences.Get("@string/CmdHeader", "") + "ver?:";
                byte[] array = Encoding.UTF8.GetBytes(s);
                await App.g_Characteristic_2.WriteAsync(array);                  // Query ESP32 version

                var receivedBytes = await App.g_Characteristic_2.ReadAsync();
                String sVer = Encoding.UTF8.GetString(receivedBytes.data, 0, receivedBytes.data.Length);
                ESP32Ver = sVer;
            }
            else
            {
                ESP32Ver = "Connect to Anteater to the the version here.";
            }

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("ReadESP32Ver Error", ex.Message, "Ok");
        }
    }
}
