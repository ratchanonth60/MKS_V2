using System.ComponentModel;
using System.Runtime.CompilerServices;
using MKS_V2.Core;

namespace MKS_V2.Models;

public class MotorState : INotifyPropertyChanged
{
    private double _positionError;
    private int _accumulatedPulses;
    private MotorEnableState _enableState;
    private bool _isStalled;

    public double PositionError
    {
        get => _positionError;
        set { _positionError = value; OnPropertyChanged(); }
    }

    public int AccumulatedPulses
    {
        get => _accumulatedPulses;
        set { _accumulatedPulses = value; OnPropertyChanged(); }
    }

    public MotorEnableState EnableState
    {
        get => _enableState;
        set { _enableState = value; OnPropertyChanged(); }
    }

    public bool IsStalled
    {
        get => _isStalled;
        set { _isStalled = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
