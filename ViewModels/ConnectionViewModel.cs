using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Models;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class ConnectionViewModel : ViewModelBase
{
    private readonly ISerialService _serialService;
    private readonly IMotorControlService _motorControlService;

    [ObservableProperty]
    private ObservableCollection<string> _availablePorts = [];

    [ObservableProperty]
    private string _selectedPort = "COM1";

    [ObservableProperty]
    private string _deviceAddressHex = "01";

    [ObservableProperty]
    private int _baudRate = 115200;

    public ObservableCollection<int> AvailableBaudRates { get; } =
    [
        9600, 19200, 25000, 38400, 57600, 115200, 256000
    ];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotConnected))]
    private bool _isConnected;

    public bool IsNotConnected => !IsConnected;

    public ConnectionViewModel(ISerialService serialService, IMotorControlService motorControlService)
    {
        _serialService = serialService;
        _motorControlService = motorControlService;
        RefreshPorts();
    }

    [RelayCommand]
    private void RefreshPorts()
    {
        AvailablePorts.Clear();
        foreach (var port in _serialService.GetAvailablePorts())
        {
            AvailablePorts.Add(port);
        }
        if (AvailablePorts.Count > 0)
        {
            SelectedPort = AvailablePorts[0];
        }
    }

    [RelayCommand]
    private void Connect()
    {
        if (byte.TryParse(DeviceAddressHex, System.Globalization.NumberStyles.HexNumber, null, out byte addr))
        {
            var config = new ConnectionConfig
            {
                PortName = SelectedPort,
                BaudRate = BaudRate,
                DeviceAddress = addr
            };

            _motorControlService.Connect(config);
            IsConnected = _serialService.IsOpen;
        }
    }

    [RelayCommand]
    private void Disconnect()
    {
        _motorControlService.Disconnect();
        IsConnected = _serialService.IsOpen;
    }
}
