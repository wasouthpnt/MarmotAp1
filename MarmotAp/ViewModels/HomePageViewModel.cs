namespace MarmotAp.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    BluetoothLEService BluetoothLEService;

    public ObservableCollection<DeviceCandidate> DeviceCandidates { get; } = new();
    public IAsyncRelayCommand GoToStatusPageAsyncCommand { get; }
    // Wyatt test
    public IAsyncRelayCommand ConnectToDeviceCandidateAsyncCommand { get; }
    //public IService FirepunkService { get; private set; }
    //public ICharacteristic FirepunkCharacteristic1 { get; private set; }
    //public ICharacteristic FirepunkCharacteristic2 { get; private set; }
    // wyatt end
    public IAsyncRelayCommand ScanNearbyDevicesAsyncCommand { get; }
    public IAsyncRelayCommand CheckBluetoothAvailabilityAsyncCommand { get; }

    public HomePageViewModel(BluetoothLEService bluetoothLEService)
    {

        Title = $"Scan and select device";

        BluetoothLEService = bluetoothLEService;

        ConnectToDeviceCandidateAsyncCommand = new AsyncRelayCommand(ConnectToDeviceCandidateAsync);

        // Wyatt test
        GoToStatusPageAsyncCommand = new AsyncRelayCommand<DeviceCandidate>(async (devicecandidate) => await GoToStatusPageAsync(devicecandidate));

        ScanNearbyDevicesAsyncCommand = new AsyncRelayCommand(ScanDevicesAsync);
        CheckBluetoothAvailabilityAsyncCommand = new AsyncRelayCommand(CheckBluetoothAvailabilityAsync);
    }

    private async Task ConnectToDeviceCandidateAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (BluetoothLEService.NewDeviceCandidateFromHomePage.Id.Equals(Guid.Empty))
        {
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
                        return;
                    }

                    if (BluetoothLEService.NewDeviceCandidateFromHomePage != null)
                    {
                        #region another device
                        if (!BluetoothLEService.Device.Id.Equals(BluetoothLEService.NewDeviceCandidateFromHomePage.Id))
                        {
                            Title = $"{BluetoothLEService.NewDeviceCandidateFromHomePage.Name}";
                            // wyatt test await DisconnectFromDeviceAsync();
                            await BluetoothLEService.ShowToastAsync($"{BluetoothLEService.Device.Name} has been disconnected.");
                        }
                        #endregion another device
                    }
                }
            }

            BluetoothLEService.Device = await BluetoothLEService.Adapter.ConnectToKnownDeviceAsync(BluetoothLEService.NewDeviceCandidateFromHomePage.Id);

            if (BluetoothLEService.Device.State == DeviceState.Connected)
            {
                App.g_Service = await BluetoothLEService.Device.GetServiceAsync(FirepunkUuids.FirepunkServiceUuid);
                if (App.g_Service != null)
                {
                    App.g_Characteristic_1 = await App.g_Service.GetCharacteristicAsync(FirepunkUuids.FirepunkCharacteristicUuid1);
                    App.g_Characteristic_2 = await App.g_Service.GetCharacteristicAsync(FirepunkUuids.FirepunkCharacteristicUuid2);

                    Title = $"{BluetoothLEService.Device.Name}";

                    //if (FirepunkCharacteristic1.CanUpdate)
                    //{
                    //FirepunkCharacteristic1.ValueUpdated += FirepunkCharacteristic1_ValueUpdated;
                    //await FirepunkCharacteristic1.StartUpdatesAsync();
                    //}
                    #region save device id to storage
                    await SecureStorage.Default.SetAsync("device_name", $"{BluetoothLEService.Device.Name}");
                    await SecureStorage.Default.SetAsync("device_id", $"{BluetoothLEService.Device.Id}");
                    #endregion save device id to storage
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

    async Task GoToStatusPageAsync(DeviceCandidate deviceCandidate)
    {
        if (IsScanning)
        {
            await BluetoothLEService.ShowToastAsync($"Bluetooth adapter is scanning. Try again.");
            return;
        }

        if (deviceCandidate == null)
        {
            return;
        }

        BluetoothLEService.NewDeviceCandidateFromHomePage = deviceCandidate;

        Title = $"{deviceCandidate.Name}";

        await ConnectToDeviceCandidateAsync();

        //await Shell.Current.GoToAsync("//StatusPage", true);
    }
    async Task ScanDevicesAsync()
    {
        if (IsScanning)
        {
            return;
        }

        if (!BluetoothLEService.BluetoothLE.IsAvailable)
        {
            Debug.WriteLine($"Bluetooth is missing.");
            await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
            return;
        }

#if ANDROID
        PermissionStatus permissionStatus = await BluetoothLEService.CheckBluetoothPermissions();
        if (permissionStatus != PermissionStatus.Granted)
        {
            permissionStatus = await BluetoothLEService.RequestBluetoothPermissions();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert($"Bluetooth LE permissions", $"Bluetooth LE permissions are not granted.", "OK");
                return;
            }
        }
#elif IOS
#elif WINDOWS
#endif

        try
        {
            if (!BluetoothLEService.BluetoothLE.IsOn)
            {
                await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                return;
            }

            IsScanning = true;

            List<DeviceCandidate> deviceCandidates = await BluetoothLEService.ScanForDevicesAsync();


            if (deviceCandidates.Count == 0)
            {
                await BluetoothLEService.ShowToastAsync($"Unable to find nearby Bluetooth LE devices. Try again.");
            }

            if (DeviceCandidates.Count > 0)
            {
                DeviceCandidates.Clear();
            }

            foreach (var deviceCandidate in deviceCandidates)
            {
                DeviceCandidates.Add(deviceCandidate);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get nearby Bluetooth LE devices: {ex.Message}");
            await Shell.Current.DisplayAlert($"Unable to get nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
        }
        finally
        {
            IsScanning = false;
        }
    }
   
    async Task CheckBluetoothAvailabilityAsync()
    {
        if (IsScanning)
        {
            return;
        }

        try
        {
            if (!BluetoothLEService.BluetoothLE.IsAvailable)
            {
                Debug.WriteLine($"Error: Bluetooth is missing.");
                await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                return;
            }

            if (BluetoothLEService.BluetoothLE.IsOn)
            {
                await Shell.Current.DisplayAlert($"Bluetooth is on", $"You are good to go.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to check Bluetooth availability: {ex.Message}");
            await Shell.Current.DisplayAlert($"Unable to check Bluetooth availability", $"{ex.Message}.", "OK");
        }
    }
}

