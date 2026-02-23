using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Core;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class SpeedControlViewModel(IMotorControlService motorService) : ViewModelBase
{
    private readonly IMotorControlService _motorService = motorService;

    [ObservableProperty]
    private ushort _speed = 1000;

    [ObservableProperty]
    private byte _acceleration = 16;

    [ObservableProperty]
    private bool _isForward = true;

    [RelayCommand]
    private void Start()
    {
        var dir = IsForward ? MotorDirection.Forward : MotorDirection.Reverse;
        _motorService.StartSpeedMode(Speed, dir, Acceleration);
    }

    [RelayCommand]
    private void Stop()
    {
        _motorService.StopSpeedMode(Acceleration);
    }
}
