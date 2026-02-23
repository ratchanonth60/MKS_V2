using System;

namespace MKS_V2.Core;

public static class MksCommandBuilder
{
    /// <summary>
    /// Calculates the Checksum for an MKS command array.
    /// Uses sum of elements truncated to byte. Note: It skips the last checksum byte itself.
    /// </summary>
    private static byte CalculateChecksum(byte[] buffer, int length)
    {
        int sum = 0;
        for (int i = 0; i < length - 1; i++)
        {
            sum += buffer[i];
        }
        return (byte)(sum & 0xFF);
    }

    /// <summary>
    /// Builds a base command for simple read/query scenarios. (Header + Addr + Cmd + Checksum)
    /// </summary>
    public static byte[] BuildQueryCommand(byte address, byte commandByte)
    {
        var buffer = new byte[4];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = commandByte;
        buffer[3] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds command to Enable or Disable Motor Drive
    /// </summary>
    public static byte[] BuildEnableControlCommand(byte address, MotorEnableState state)
    {
        var buffer = new byte[5];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.SetEnable;
        buffer[3] = (byte)state;
        buffer[4] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds speed mode control command
    /// </summary>
    public static byte[] BuildSpeedModeCommand(byte address, ushort speed, MotorDirection direction, byte acceleration)
    {
        var speedBytes = BitConverter.GetBytes(speed);
        speedBytes[1] |= (byte)direction; // Combine direction flags based 

        var buffer = new byte[7];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.SpeedModeStart;
        buffer[3] = speedBytes[1];
        buffer[4] = speedBytes[0];
        buffer[5] = acceleration;
        buffer[6] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Stop current movement
    /// </summary>
    public static byte[] BuildStopMovementCommand(byte address, byte commandByte, byte stopAcceleration)
    {
        var buffer = new byte[7];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = commandByte; // could be SpeedModeStop or PositionModeStop
        buffer[3] = 0x00;
        buffer[4] = 0x00;
        buffer[5] = stopAcceleration;
        buffer[6] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Position control using Pulses (Relative or Absolute)
    /// </summary>
    public static byte[] BuildPositionPulsesCommand(byte address, PositionControlMode mode, int pulses, ushort speed, MotorDirection direction, byte acceleration)
    {
        byte cmdByte = mode == PositionControlMode.Relative
                       ? MksProtocolConstants.Commands.PositionModeStart  // Form1: 0xFD
                       : MksProtocolConstants.Commands.PositionModeAbsolutePulsesControl; // Form1: 0xFE

        var speedBytes = BitConverter.GetBytes(speed);
        speedBytes[1] |= (byte)direction;

        var pulseBytes = BitConverter.GetBytes(pulses);

        var buffer = new byte[11];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = cmdByte;
        buffer[3] = speedBytes[1];
        buffer[4] = speedBytes[0];
        buffer[5] = acceleration;
        buffer[6] = pulseBytes[3];
        buffer[7] = pulseBytes[2];
        buffer[8] = pulseBytes[1];
        buffer[9] = pulseBytes[0];
        buffer[10] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Position control using Axis Degrees mapped to int values (Relative or Absolute)
    /// Form1 maps Axis degrees to value via (AxisText * 16384 / 360)
    /// </summary>
    public static byte[] BuildPositionAxisCommand(byte address, PositionControlMode mode, int mappedAxisValue, ushort speed, byte acceleration)
    {
        byte cmdByte = mode == PositionControlMode.Relative
            ? MksProtocolConstants.Commands.PositionModeRelativeControl // 0xF4
            : MksProtocolConstants.Commands.PositionModeAbsoluteControl; // 0xF5

        var speedBytes = BitConverter.GetBytes(speed);
        // Form1 logic does not apply direction explicitely to Speed[1] here for Axis
        var axisBytes = BitConverter.GetBytes(mappedAxisValue);

        var buffer = new byte[11];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = cmdByte;
        buffer[3] = speedBytes[1];
        buffer[4] = speedBytes[0];
        buffer[5] = acceleration;
        buffer[6] = axisBytes[3];
        buffer[7] = axisBytes[2];
        buffer[8] = axisBytes[1];
        buffer[9] = axisBytes[0];
        buffer[10] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Setting subdivision value
    /// </summary>
    public static byte[] BuildSetSubdivisionCommand(byte address, ushort subdivision)
    {
        byte subVal = (subdivision == 256) ? (byte)0x00 : (byte)subdivision;
        return BuildSingleBytePayloadCommand(address, MksProtocolConstants.Commands.SetSubdivision, subVal);
    }

    /// <summary>
    /// Builds a command that sends a single byte of payload.
    /// Useful for many system parameters (e.g. Baud Rate, Comm Address, Accelerations, Limit Remap, etc.)
    /// </summary>
    public static byte[] BuildSingleBytePayloadCommand(byte address, byte commandByte, byte payloadByte)
    {
        var buffer = new byte[5];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = commandByte;
        buffer[3] = payloadByte;
        buffer[4] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds command to set current (Hold/Run)
    /// </summary>
    public static byte[] BuildSetCurrentCommand(byte address, ushort currentMa)
    {
        var currentBytes = BitConverter.GetBytes(currentMa);
        var buffer = new byte[6];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.SetCurrent;
        buffer[3] = currentBytes[1];
        buffer[4] = currentBytes[0];
        buffer[5] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds limit parameters command
    /// </summary>
    public static byte[] BuildSetLimitParametersCommand(byte address, LimitTriggerLevel triggerLevel, MotorDirection homeDir, ushort speed, StateSwitch homeEnable)
    {
        var speedBytes = BitConverter.GetBytes(speed);
        var buffer = new byte[9];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.SetLimitParameters;
        buffer[3] = (byte)triggerLevel;
        buffer[4] = (byte)homeDir;
        buffer[5] = speedBytes[1];
        buffer[6] = speedBytes[0];
        buffer[7] = (byte)homeEnable;
        buffer[8] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds No Limit Switch Home Command
    /// </summary>
    public static byte[] BuildTriggerNoLimitSwitchHomeCommand(byte address, uint returnDistance, NoLimitSwitchHomeMode mode, ushort currentMa)
    {
        var distBytes = BitConverter.GetBytes(returnDistance);
        var currentBytes = BitConverter.GetBytes(currentMa);

        var buffer = new byte[11];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.TriggerNoLimitSwitchHome;
        buffer[3] = distBytes[3];
        buffer[4] = distBytes[2];
        buffer[5] = distBytes[1];
        buffer[6] = distBytes[0];
        buffer[7] = (byte)mode;
        buffer[8] = currentBytes[1];
        buffer[9] = currentBytes[0];
        buffer[10] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }

    /// <summary>
    /// Builds Single Turn Home Command
    /// </summary>
    public static byte[] BuildTriggerSingleTurnHomeCommand(byte address, SingleTurnHomeMode mode, SingleTurnZeroSetting setZero, MotorDirection dir, byte speed)
    {
        var buffer = new byte[8];
        buffer[0] = MksProtocolConstants.FrameHeader;
        buffer[1] = address;
        buffer[2] = MksProtocolConstants.Commands.TriggerSingleTurnHome;
        buffer[3] = (byte)mode;
        buffer[4] = (byte)setZero;
        buffer[5] = speed;
        buffer[6] = (byte)dir;
        buffer[7] = CalculateChecksum(buffer, buffer.Length);
        return buffer;
    }
}
