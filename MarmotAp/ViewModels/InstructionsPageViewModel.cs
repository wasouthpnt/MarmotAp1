namespace MarmotAp.ViewModels;

public partial class InstructionsPageViewModel : BaseViewModel
{
    public BluetoothLEService BluetoothLEService { get; private set; }
    public InstructionsPageViewModel(BluetoothLEService bluetoothLEService)
    {
        Title = $"Instructions";

        BluetoothLEService = bluetoothLEService;
    }
}

