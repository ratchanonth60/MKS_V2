using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MKS_V2.Core;
using MKS_V2.Models;
using MKS_V2.Services;

namespace MKS_V2.ViewModels;

public partial class LimitSettingsViewModel : ViewModelBase
{
    private readonly IMotorControlService _motorControlService;

    // View Model state properties mapping to Form1 Limit parameters
    [ObservableProperty]
    private LimitTriggerLevel _limitTriggerLevel = LimitTriggerLevel.Low;

    [ObservableProperty]
    private MotorDirection _homeDirection = MotorDirection.Reverse;

    [ObservableProperty]
    private StateSwitch _homeEnable = StateSwitch.Disable;

    [ObservableProperty]
    private ushort _homeSpeed = 500;

    [ObservableProperty]
    private SingleTurnHomeMode _singleTurnHomeMode = SingleTurnHomeMode.Disable;

    [ObservableProperty]
    private SingleTurnZeroSetting _singleTurnZeroSetting = SingleTurnZeroSetting.Clear0;

    [ObservableProperty]
    private MotorDirection _singleTurnHomeDirection = MotorDirection.Reverse;

    [ObservableProperty]
    private byte _singleTurnHomeSpeed = 10;

    // Commands
    [RelayCommand]
    private void SaveLimitParameters()
    {
        _motorControlService.SetLimitParameters(LimitTriggerLevel, HomeDirection, HomeSpeed, HomeEnable);
    }

    [RelayCommand]
    private void TriggerLimitHome()
    {
        _motorControlService.TriggerLimitHome();
    }

    [RelayCommand]
    private void TriggerDirectHome()
    {
        _motorControlService.TriggerDirectHome();
    }

    [RelayCommand]
    private void TriggerSingleTurnHome()
    {
        _motorControlService.TriggerSingleTurnHome(
            SingleTurnHomeMode,
            SingleTurnZeroSetting,
            SingleTurnHomeDirection,
            SingleTurnHomeSpeed
        );
    }

    public LimitSettingsViewModel(IMotorControlService motorControlService)
    {
        _motorControlService = motorControlService;
    }
}
