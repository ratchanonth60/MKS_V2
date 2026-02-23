using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Core;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class SettingsViewModel(IMotorControlService motorService) : ViewModelBase
{
    private readonly IMotorControlService _motorService = motorService;

    [ObservableProperty]
    private ushort _selectedSubdivision = 16;

    public ObservableCollection<ushort> AvailableSubdivisions { get; } =
    [
        1, 2, 4, 8, 16, 32, 64, 128, 256
    ];

    [ObservableProperty]
    private ushort _runningCurrent = 1500;

    // 0 = 10%, 8 = 90%
    [ObservableProperty]
    private byte _holdingCurrentIndex = 4; // Default 50%

    [ObservableProperty]
    private UartBaudRate _baudRate = UartBaudRate.Baud38400;

    [ObservableProperty]
    private byte _commAddress = 1;

    [ObservableProperty]
    private MotorDirection _motorDirection = MotorDirection.Forward;

    [ObservableProperty]
    private byte _startAcceleration = 16;

    [ObservableProperty]
    private byte _stopAcceleration = 16;

    [RelayCommand]
    private void ApplySubdivision()
    {
        _motorService.SetSubdivision(SelectedSubdivision);
    }

    [RelayCommand]
    private void ApplyRunningCurrent()
    {
        _motorService.SetCurrent(RunningCurrent);
    }

    [RelayCommand]
    private void ApplyHoldingCurrent()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetHoldCurrent, HoldingCurrentIndex);
    }

    [RelayCommand]
    private void ApplyBaudRate()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetUartBaudRate, (byte)BaudRate);
    }

    [RelayCommand]
    private void ApplyCommAddress()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetCommAddress, CommAddress);
    }

    [RelayCommand]
    private void ApplyMotorDirection()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetMotorDirection, (byte)MotorDirection);
    }

    [RelayCommand]
    private void ApplyStartAcceleration()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetStartAcceleration, StartAcceleration);
    }

    [RelayCommand]
    private void ApplyStopAcceleration()
    {
        _motorService.SetSystemParameter(MksProtocolConstants.Commands.SetStopAcceleration, StopAcceleration);
    }
}
