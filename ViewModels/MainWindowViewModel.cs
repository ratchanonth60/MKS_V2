using CommunityToolkit.Mvvm.ComponentModel;

namespace MKS_V2.ViewModels;

public partial class MainWindowViewModel(
    ConnectionViewModel connection,
    SpeedControlViewModel speedControl,
    PositionControlViewModel positionControl,
    SettingsViewModel settings,
    MonitorViewModel monitor,
    LimitSettingsViewModel limitSettings) : ViewModelBase
{
    public ConnectionViewModel Connection { get; } = connection;
    public SpeedControlViewModel SpeedControl { get; } = speedControl;
    public PositionControlViewModel PositionControl { get; } = positionControl;
    public SettingsViewModel Settings { get; } = settings;
    public MonitorViewModel Monitor { get; } = monitor;
    public LimitSettingsViewModel LimitSettings { get; } = limitSettings;
}
