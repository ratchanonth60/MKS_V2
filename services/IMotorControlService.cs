using MKS_V2.Core;
using MKS_V2.Models;

namespace MKS_V2.Services;

public interface IMotorControlService
{
    void Connect(ConnectionConfig config);
    void Disconnect();

    void SetEnableState(MotorEnableState state);
    void StartSpeedMode(ushort speed, MotorDirection direction, byte acceleration);
    void StopSpeedMode(byte stopAcceleration);
    void MoveAbsolutePulses(int pulses, ushort speed, byte acceleration);
    void MoveRelativePulses(int pulses, ushort speed, MotorDirection direction, byte acceleration);
    void MoveAbsoluteAxis(int axisMappedValue, ushort speed, byte acceleration);
    void MoveRelativeAxis(int axisMappedValue, ushort speed, byte acceleration);
    void SetSubdivision(ushort subdivision);
    void SetCurrent(ushort currentMa);
    void SetSystemParameter(byte commandByte, byte payloadByte);

    void SetLimitParameters(LimitTriggerLevel triggerLevel, MotorDirection homeDir, ushort speed, StateSwitch homeEnable);
    void TriggerLimitHome();
    void TriggerDirectHome();
    void TriggerSingleTurnHome(SingleTurnHomeMode mode, SingleTurnZeroSetting setZero, MotorDirection dir, byte speed);

    void QueryStatus();
    void QueryParameter(byte commandByte);
}
