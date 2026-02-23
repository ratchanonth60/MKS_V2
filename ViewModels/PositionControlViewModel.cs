using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Core;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class PositionControlViewModel(IMotorControlService motorService) : ViewModelBase
{
    private readonly IMotorControlService _motorService = motorService;

    [ObservableProperty]
    private int _targetValue = 3200; // Pulses or Axis

    [ObservableProperty]
    private ushort _speed = 1000;

    [ObservableProperty]
    private byte _acceleration = 16;

    [ObservableProperty]
    private bool _isForward = true;

    [ObservableProperty]
    private string _selectedMode = "Relative Pulses";

    public ObservableCollection<string> Modes { get; } =
    [
        "Relative Pulses",
        "Absolute Pulses",
        "Relative Axis",
        "Absolute Axis"
    ];

    [RelayCommand]
    private void Start()
    {
        var dir = IsForward ? MotorDirection.Forward : MotorDirection.Reverse;

        switch (SelectedMode)
        {
            case "Relative Pulses":
                _motorService.MoveRelativePulses(TargetValue, Speed, dir, Acceleration);
                break;
            case "Absolute Pulses":
                _motorService.MoveAbsolutePulses(TargetValue, Speed, Acceleration);
                break;
            case "Relative Axis":
                _motorService.MoveRelativeAxis(TargetValue, Speed, Acceleration);
                break;
            case "Absolute Axis":
                _motorService.MoveAbsoluteAxis(TargetValue, Speed, Acceleration);
                break;
        }
    }
}
