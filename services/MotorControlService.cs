using System;
using MKS_V2.Core;
using MKS_V2.Models;

namespace MKS_V2.Services;

public class MotorControlService : IMotorControlService
{
    private readonly ISerialService _serialService;
    private ConnectionConfig? _currentConfig;

    public MotorControlService(ISerialService serialService)
    {
        _serialService = serialService;
        _serialService.DataReceived += SerialService_DataReceived;
    }

    private void SerialService_DataReceived(object? sender, byte[] e)
    {
        var result = MksResponseParser.Parse(e);
        if (!result.IsValid) return;

        // Route the result to models/viewmodels using an event aggregator or direct events here.
        // We will wire this to MotorState later.
    }

    public void Connect(ConnectionConfig config)
    {
        _currentConfig = config;
        _serialService.Connect(config.PortName, config.BaudRate);
    }

    public void Disconnect()
    {
        _serialService.Disconnect();
    }

    public void SetEnableState(MotorEnableState state)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildEnableControlCommand(_currentConfig.DeviceAddress, state);
        _serialService.Write(cmd);
    }

    public void StartSpeedMode(ushort speed, MotorDirection direction, byte acceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildSpeedModeCommand(_currentConfig.DeviceAddress, speed, direction, acceleration);
        _serialService.Write(cmd);
    }

    public void StopSpeedMode(byte stopAcceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildStopMovementCommand(_currentConfig.DeviceAddress, MksProtocolConstants.Commands.SpeedModeStop, stopAcceleration);
        _serialService.Write(cmd);
    }

    public void MoveAbsolutePulses(int pulses, ushort speed, byte acceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildPositionPulsesCommand(_currentConfig.DeviceAddress, PositionControlMode.Absolute, pulses, speed, MotorDirection.Forward, acceleration);
        _serialService.Write(cmd);
    }

    public void MoveRelativePulses(int pulses, ushort speed, MotorDirection direction, byte acceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildPositionPulsesCommand(_currentConfig.DeviceAddress, PositionControlMode.Relative, pulses, speed, direction, acceleration);
        _serialService.Write(cmd);
    }

    public void MoveAbsoluteAxis(int axisMappedValue, ushort speed, byte acceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildPositionAxisCommand(_currentConfig.DeviceAddress, PositionControlMode.Absolute, axisMappedValue, speed, acceleration);
        _serialService.Write(cmd);
    }

    public void MoveRelativeAxis(int axisMappedValue, ushort speed, byte acceleration)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildPositionAxisCommand(_currentConfig.DeviceAddress, PositionControlMode.Relative, axisMappedValue, speed, acceleration);
        _serialService.Write(cmd);
    }

    public void SetSubdivision(ushort subdivision)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildSetSubdivisionCommand(_currentConfig.DeviceAddress, subdivision);
        _serialService.Write(cmd);
    }

    public void SetCurrent(ushort currentMa)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildSetCurrentCommand(_currentConfig.DeviceAddress, currentMa);
        _serialService.Write(cmd);
    }

    public void SetSystemParameter(byte commandByte, byte payloadByte)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildSingleBytePayloadCommand(_currentConfig.DeviceAddress, commandByte, payloadByte);
        _serialService.Write(cmd);
    }

    public void SetLimitParameters(LimitTriggerLevel triggerLevel, MotorDirection homeDir, ushort speed, StateSwitch homeEnable)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildSetLimitParametersCommand(_currentConfig.DeviceAddress, triggerLevel, homeDir, speed, homeEnable);
        _serialService.Write(cmd);
    }

    public void TriggerLimitHome()
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildQueryCommand(_currentConfig.DeviceAddress, MksProtocolConstants.Commands.TriggerLimitHome);
        _serialService.Write(cmd);
    }

    public void TriggerDirectHome()
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildQueryCommand(_currentConfig.DeviceAddress, MksProtocolConstants.Commands.TriggerDirectHome);
        _serialService.Write(cmd);
    }

    public void TriggerSingleTurnHome(SingleTurnHomeMode mode, SingleTurnZeroSetting setZero, MotorDirection dir, byte speed)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildTriggerSingleTurnHomeCommand(_currentConfig.DeviceAddress, mode, setZero, dir, speed);
        _serialService.Write(cmd);
    }

    public void QueryStatus()
    {
        if (_currentConfig == null) return;

        // E.g. query enable state
        var cmd = MksCommandBuilder.BuildQueryCommand(_currentConfig.DeviceAddress, MksProtocolConstants.Commands.ReadEnableState);
        _serialService.Write(cmd);
    }

    public void QueryParameter(byte commandByte)
    {
        if (_currentConfig == null) return;
        var cmd = MksCommandBuilder.BuildQueryCommand(_currentConfig.DeviceAddress, commandByte);
        _serialService.Write(cmd);
    }
}
