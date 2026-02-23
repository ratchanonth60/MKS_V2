using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Core;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class MonitorViewModel(IMotorControlService motorService) : ViewModelBase
{
    private readonly IMotorControlService _motorService = motorService;

    // Real-time Status Readouts
    [ObservableProperty]
    private string _encoderCarry = "0";

    [ObservableProperty]
    private string _accumulatedPulses = "0";

    [ObservableProperty]
    private string _rawAccumulatedEncoder = "0";

    [ObservableProperty]
    private string _positionAngleError = "0";

    [ObservableProperty]
    private string _enableState = "Unknown";

    [ObservableProperty]
    private string _autoZeroStatus = "Unknown";

    [ObservableProperty]
    private string _stallFlag = "Unknown";

    [ObservableProperty]
    private string _ioPortState = "0000";

    [ObservableProperty]
    private string _hardwareVersion = "Unknown";

    [ObservableProperty]
    private string _statusMessage = "Ready";

    // Controls
    [RelayCommand]
    private void EnableMotor()
    {
        _motorService.SetEnableState(MotorEnableState.Enable);
        StatusMessage = "Motor Enabled";
    }

    [RelayCommand]
    private void DisableMotor()
    {
        _motorService.SetEnableState(MotorEnableState.Disable);
        StatusMessage = "Motor Disabled";
    }

    [RelayCommand]
    private void ClearStallFlag()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ClearStallFlag);
        StatusMessage = "Stall Flag Cleared";
    }

    [RelayCommand]
    private void RestoreFactoryConfig()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.RestoreFactoryConfig);
        StatusMessage = "Factory Config Restored";
    }

    [RelayCommand]
    private void ResetMotor()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ResetRestartMotor);
        StatusMessage = "Motor Reset";
    }

    // Queries
    [RelayCommand]
    private void QueryEncoderCarry()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadEncoderCarry);
    }

    [RelayCommand]
    private void QueryAccumulatedPulses()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadAccumulatedPulses);
    }

    [RelayCommand]
    private void QueryRawAccumulatedEncoder()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadRawAccumulatedEncoder);
    }

    [RelayCommand]
    private void QueryPositionAngleError()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadPositionAngleError);
    }

    [RelayCommand]
    private void QueryEnableState()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadEnableState);
    }

    [RelayCommand]
    private void QueryAutoZeroStatus()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadAutoZeroStatus);
    }

    [RelayCommand]
    private void QueryStallFlag()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadStallFlag);
    }

    [RelayCommand]
    private void QueryIoPortState()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadIoPortState);
    }

    [RelayCommand]
    private void QueryHardwareVersion()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadHardwareVersion);
    }

    [RelayCommand]
    private void QueryAllStatus()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadAllStatus);
    }

    [RelayCommand]
    private void QueryAllParams()
    {
        _motorService.QueryParameter(MksProtocolConstants.Commands.ReadAllParams);
    }
}
