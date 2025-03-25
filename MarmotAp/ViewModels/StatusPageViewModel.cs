using System.Text;

namespace MarmotAp.ViewModels;

public partial class StatusPageViewModel : BaseViewModel
{
    private String _ID = "";
    const int dataSize = 36;            // Max Bytes for raw data (Packet10hz)
    const int notifyCallBackMax = 20;   // Max Bytes from registered notiify callback
    private byte[] data = new byte[dataSize];
    bool bHeader = false;

    public BluetoothLEService BluetoothLEService { get; private set; }
    public IAsyncRelayCommand ConnectToDeviceCandidateAsyncCommand { get; }
    public IAsyncRelayCommand DisconnectFromDeviceAsyncCommand { get; }
    public IService FirepunkService { get; private set; }
    public ICharacteristic FirepunkCharacteristic1 { get; private set; }
    public ICharacteristic FirepunkCharacteristic2 { get; private set; }
    
    public StatusPageViewModel(BluetoothLEService bluetoothLEService)
    {
        //Title = $"Status Page";

        BluetoothLEService = bluetoothLEService;

        ConnectToDeviceCandidateAsyncCommand = new AsyncRelayCommand(ConnectToDeviceCandidateAsync);

        DisconnectFromDeviceAsyncCommand = new AsyncRelayCommand(DisconnectFromDeviceAsync);
    }

    [ObservableProperty]
    string iDValue;

    [ObservableProperty]
    string mPHValue;

    [ObservableProperty]
    string tuneValue;

    [ObservableProperty]
    string gearValue;

    [ObservableProperty]
    string lockupValue;

    [ObservableProperty]
    DateTimeOffset timestamp;

    [ObservableProperty]
    bool isNotRunning = true; // This hides the connect button when already running

    private async Task ConnectToDeviceCandidateAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (BluetoothLEService.NewDeviceCandidateFromHomePage.Id.Equals(Guid.Empty))
        {
            await Shell.Current.DisplayAlert("Sorry", "You must first scan and connect to device", "Ok");
            await Shell.Current.GoToAsync("//HomePage", true);
            return;
            #region read device id from storage
            var device_name = await SecureStorage.Default.GetAsync("device_name");
            var device_id = await SecureStorage.Default.GetAsync("device_id");
            if (!string.IsNullOrEmpty(device_id))
            {
                BluetoothLEService.NewDeviceCandidateFromHomePage.Name = device_name;
                BluetoothLEService.NewDeviceCandidateFromHomePage.Id = Guid.Parse(device_id);
            }
            #endregion read device id from storage
            else
            {
                await BluetoothLEService.ShowToastAsync($"Select a Bluetooth LE device first. Try again.");
                return;
            }
        }

        if (!BluetoothLEService.BluetoothLE.IsOn)
        {
            await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
            return;
        }

        if (BluetoothLEService.Adapter.IsScanning)
        {
            await BluetoothLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
            return;
        }

        try
        {
            IsBusy = true;

            if (BluetoothLEService.Device != null)
            {
                if (BluetoothLEService.Device.State == DeviceState.Connected)
                {
                    if (BluetoothLEService.Device.Id.Equals(BluetoothLEService.NewDeviceCandidateFromHomePage.Id))
                    {
                        await BluetoothLEService.ShowToastAsync($"{BluetoothLEService.Device.Name} is already connected.");
                        //return;
                    }

                    if (BluetoothLEService.NewDeviceCandidateFromHomePage != null)
                    {
                        #region another device
                        if (!BluetoothLEService.Device.Id.Equals(BluetoothLEService.NewDeviceCandidateFromHomePage.Id))
                        {
                            Title = $"{BluetoothLEService.NewDeviceCandidateFromHomePage.Name}";
                            await DisconnectFromDeviceAsync();
                            await BluetoothLEService.ShowToastAsync($"{BluetoothLEService.Device.Name} has been disconnected.");
                        }
                        #endregion another device
                    }
                }
            }

            BluetoothLEService.Device = await BluetoothLEService.Adapter.ConnectToKnownDeviceAsync(BluetoothLEService.NewDeviceCandidateFromHomePage.Id);

            if (BluetoothLEService.Device.State == DeviceState.Connected)
            {
                
                FirepunkService = await BluetoothLEService.Device.GetServiceAsync(FirepunkUuids.FirepunkServiceUuid);
                if (FirepunkService != null)
                {
                    //FirepunkCharacteristic1 = await FirepunkService.GetCharacteristicAsync(FirepunkUuids.FirepunkCharacteristicUuid1);
                    //FirepunkCharacteristic2 = await FirepunkService.GetCharacteristicAsync(FirepunkUuids.FirepunkCharacteristicUuid2);

                    Title = $"{BluetoothLEService.Device.Name}";

                    if (App.g_Characteristic_1.CanUpdate)
                    {
                        App.g_Characteristic_1.ValueUpdated += FirepunkCharacteristic1_ValueUpdated;
                        await App.g_Characteristic_1.StartUpdatesAsync();
                    }

                }
                
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to connect to {BluetoothLEService.NewDeviceCandidateFromHomePage.Name} {BluetoothLEService.NewDeviceCandidateFromHomePage.Id}: {ex.Message}.");
            await Shell.Current.DisplayAlert($"{BluetoothLEService.NewDeviceCandidateFromHomePage.Name}", $"Unable to connect to {BluetoothLEService.NewDeviceCandidateFromHomePage.Name}.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void FirepunkCharacteristic1_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
    {
        IsNotRunning = false;

        var receivedBytes = e.Characteristic.Value;
        string _charStr = "";                                                                           // in the following section the received bytes will be displayed in different ways (you can select the method you need)
        if (receivedBytes != null)
        {
            string s = Preferences.Get("@string/CmdHeader", "");
            s = s.ToLower();
            _charStr = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);

            if (_charStr == s && bHeader == false)
            {
                bHeader = true;
            }
            if ((receivedBytes[0] == 0x0A) && bHeader)
            {
                // Save first 20 bytes
                receivedBytes.CopyTo(data, 0);
                bHeader = false;
            }
            else
            {
                // Combine next 16 bytes with previously saved 20 bytes data for the full 36 bytes
                if (data[0] == 0x0A)
                {
                    receivedBytes.CopyTo(data, notifyCallBackMax);
                   
                    // Parse into a Packet10hz
                    Packet10hz p = new Packet10hz();
                    p = Parse.ToPacket10hz(data);

                    // save to App for access throughout
                    App.packet10Hz = p;

                    MPHValue = p.mph.ToString();
                    string sMPH = Preferences.Get("MPH","0");
                    if (sMPH != MPHValue)
                    {
                        Preferences.Set("MPH", MPHValue);
                    }

                    TuneValue = "Tune #" + p.loadedTune.ToString();
                    GearValue = "Gear: " + p.gear.ToString();
                    string sLockup = p.LU == true ? "Yes" : "No";
                    LockupValue = "Lockup: " + sLockup;
                    IDValue = _ID.ToString();

                    Array.Clear(data, 0, dataSize);
                }
            }

        }

        Timestamp = DateTimeOffset.Now.LocalDateTime;
    }
    private async Task DisconnectFromDeviceAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (BluetoothLEService.Device == null)
        {
            await BluetoothLEService.ShowToastAsync($"Nothing to do.");
            return;
        }

        if (!BluetoothLEService.BluetoothLE.IsOn)
        {
            await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
            return;
        }

        if (BluetoothLEService.Adapter.IsScanning)
        {
            await BluetoothLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
            return;
        }

        if (BluetoothLEService.Device.State == DeviceState.Disconnected)
        {
            await BluetoothLEService.ShowToastAsync($"{BluetoothLEService.Device.Name} is already disconnected.");
            return;
        }

        try
        {
            IsBusy = true;

            await App.g_Characteristic_1.StopUpdatesAsync();

            // wyatt test
            //await BluetoothLEService.Adapter.DisconnectDeviceAsync(BluetoothLEService.Device);

            App.g_Characteristic_1.ValueUpdated -= FirepunkCharacteristic1_ValueUpdated;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to disconnect from {BluetoothLEService.Device.Name} {BluetoothLEService.Device.Id}: {ex.Message}.");
            await Shell.Current.DisplayAlert($"{BluetoothLEService.Device.Name}", $"Unable to disconnect from {BluetoothLEService.Device.Name}.", "OK");
        }
        finally
        {
            Title = "Status Page";
            MPHValue = "";
            Timestamp = DateTimeOffset.MinValue;
            IsBusy = false;
            IsNotRunning = true;
            // wyatt test
            //BluetoothLEService.Device?.Dispose();
            //BluetoothLEService.Device = null;
            await Shell.Current.GoToAsync("//HomePage", true);
        }
    }
}

