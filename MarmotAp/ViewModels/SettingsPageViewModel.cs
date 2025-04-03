using System.Text;

namespace MarmotAp.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        public BluetoothLEService BluetoothLEService { get; private set; }
        public IAsyncRelayCommand SaveNewDeviceNameAsyncCommand { get; }
        public IAsyncRelayCommand SetSystemThemeAsyncCommand { get; }
        public IAsyncRelayCommand SetLightThemeAsyncCommand { get; }
        public IAsyncRelayCommand SetDarkThemeAsyncCommand { get; }

        //public IAsyncRelayCommand ScanNearbyDevicesAsyncCommand { get; }
        //public IAsyncRelayCommand RadioButtonCheckChangedCommand { get; }

        //public string name1;
        //public string Name1
        //{
        //    get => name1;
        //    set
        //    {
        //        name1 = value;
        //        OnPropertyChanged(nameof(Name1));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //public ICommand ChangeCurrentPlayer2Command { get; set; }



        public SettingsPageViewModel(BluetoothLEService bluetoothLEService)
        {
            Title = $"Settings";
            BluetoothLEService = bluetoothLEService;

            CurrentDevName = Preferences.Get("@string/CmdHeader", "");

            SetSystemThemeAsyncCommand = new AsyncRelayCommand(SetSystemThemeAsync);
            SetLightThemeAsyncCommand = new AsyncRelayCommand(SetLightThemeAsync);
            SetDarkThemeAsyncCommand = new AsyncRelayCommand(SetDarkThemeAsync);

            SaveNewDeviceNameAsyncCommand = new AsyncRelayCommand(SaveNewDeviceNameAsync);

        }

        [ObservableProperty]
        string newDevName;

        [ObservableProperty]
        string currentDevName;


        //public void ChangeCurrentPlayer2(object content)
        //{
        //    var radiobutton = content as RadioButton;

        //    if ("System" == radiobutton.Value.ToString())
        //    {
        //        //Name1 = radiobutton.Value.ToString();
        //        if (radiobutton.IsChecked)
        //        {
        //            Settings.Theme = 0;
        //            TheTheme.SetTheme();
        //        }
        //    }
        //    else if ("Light" == radiobutton.Value.ToString())
        //    {
        //        Settings.Theme = 1;
        //        TheTheme.SetTheme();
        //    }
        //    else if ("Dark" == radiobutton.Value.ToString())
        //    {
        //        Settings.Theme = 2;
        //        TheTheme.SetTheme();
        //    }
        //}

        private async Task SetSystemThemeAsync()
        {
            Settings.Theme = 0;
            TheTheme.SetTheme();
        }
        private async Task SetLightThemeAsync()
        {
            Settings.Theme = 1;
            TheTheme.SetTheme();
        }
        private async Task SetDarkThemeAsync()
        {
            Settings.Theme = 2;
            TheTheme.SetTheme();
        }

        private async Task SaveNewDeviceNameAsync()
        {
            if (BluetoothLEService == null)
            {
                await Shell.Current.DisplayAlert("No device", "Connect First", "Ok");
            }

            if (App.g_Characteristic_2 == null)
            {
                await Shell.Current.DisplayAlert("Not Connected", "Connect First", "Ok");
                return;
            }
            if (CurrentDevName.Replace("_", "") == NewDevName)
            {
                await Shell.Current.DisplayAlert("?", "Nothing to change", "Ok");
                return;
            }
            //WriteSignature();
        }

        private async void WriteSignature()
        {
            try
            {
                string sdev = Preferences.Get("@string/CmdHeader", "");
                byte[] array = Encoding.UTF8.GetBytes("dev?:" + sdev);   // Set the new unique device name in the ESP32
                await App.g_Characteristic_2.WriteAsync(array);          // Set the device signature (DevName)
                Thread.Sleep(50);

                var receivedBytes = await App.g_Characteristic_2.ReadAsync();
                string s = Encoding.UTF8.GetString(receivedBytes.data, 0, receivedBytes.data.Length);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Send Error", ex.Message, "Cancel");
            }
        }

    }
}
