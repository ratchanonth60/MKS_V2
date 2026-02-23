using System.Collections.Generic;

namespace MKS_V2.Core;

public static class MksProtocolConstants
{
    public const byte FrameHeader = 0xFA;

    /// <summary>
    /// Definitions based on `QueryCommand` and opcode descriptions from Form1.cs
    /// </summary>
    public static class Commands
    {
        // === Motor Control & Status Queries (GB2) ===
        public const byte ReadEncoderCarry = 0x30;
        public const byte ReadAccumulatedPulses = 0x33;
        public const byte ReadPositionAngleError = 0x39;
        public const byte ReadEnableState = 0x3A;
        public const byte ReadAutoZeroStatus = 0x3B;
        public const byte ReadStallFlag = 0x3E;
        public const byte ReadIoPortState = 0x34;
        public const byte ReadRawAccumulatedEncoder = 0x35;
        public const byte ReadHardwareVersion = 0x40;
        public const byte ReadAllParams = 0x47;
        public const byte ReadAllStatus = 0x48;

        // === Basic Motor Controls (GB4) ===
        public const byte SetEnable = 0xF3;       // For Enable
        public const byte SetDisable = 0xF3;      // Note: same code, param determines enable/disable
        public const byte SpeedModeStart = 0xF6;
        public const byte SpeedModeStop = 0xF6;
        public const byte PositionModeStart = 0xFD;
        public const byte PositionModeStop = 0xFD;
        public const byte PositionModeRelativeControl = 0xF4;
        public const byte PositionModeAbsoluteControl = 0xF5;
        public const byte PositionModeAbsolutePulsesControl = 0xFE;
        public const byte EmergencyStop = 0xF7;
        public const byte ResetRestartMotor = 0x41;

        // === System & Comm Parameters (GB3) ===
        public const byte CalibrateEncoder = 0x80;
        public const byte SetMotorType = 0x81;
        public const byte SetWorkMode = 0x82;
        public const byte SetCurrent = 0x83;
        public const byte SetSubdivision = 0x84;
        public const byte SetEnablerConfig = 0x85;
        public const byte SetMotorDirection = 0x86;
        public const byte SetAutoScreenOff = 0x87;
        public const byte SetStallProtect = 0x88;
        public const byte SetSubdivisionInterpolation = 0x89;
        public const byte SetUartBaudRate = 0x8A;
        public const byte SetCommAddress = 0x8B;
        public const byte SetSlaveResponse = 0x8C;
        public const byte SetSlaveGroup = 0x8D;
        public const byte SetKeyLock = 0x8F;
        public const byte SetHoldCurrent = 0x9B;
        public const byte RestoreFactoryConfig = 0x3F;
        public const byte ClearStallFlag = 0x3D;
        public const byte SetAutoRunOnPowerUp = 0xFF; // Start/Stop

        // === PID & Accelerations ===
        public const byte SetPositionKp = 0xA1;
        public const byte SetPositionKi = 0xA2;
        public const byte SetPositionKd = 0xA3;
        public const byte SetStartAcceleration = 0xA4;
        public const byte SetStopAcceleration = 0xA5;

        // === Limits, Homing & IO (GB5) ===
        public const byte SetLimitParameters = 0x90; // Includes Home Mode Enable/Trig etc
        public const byte TriggerLimitHome = 0x91;
        public const byte TriggerDirectHome = 0x92;
        public const byte SetHomeDirection = 0x93;
        public const byte TriggerNoLimitSwitchHome = 0x94;
        public const byte TriggerSingleTurnHome = 0x9A;
        public const byte SetLimitRemap = 0x9E;
        public const byte SetEnHomeAndPositionProtect = 0x9D;
        public const byte WriteIoPort = 0x36;
    }
}
