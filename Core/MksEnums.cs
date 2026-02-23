namespace MKS_V2.Core;

public enum MotorDirection : byte
{
    Reverse = 0x00,
    Forward = 0x80
}

public enum MotorEnableState : byte
{
    Disable = 0x00,
    Enable = 0x01
}

public enum HardwareVersion
{
    UnknownDrive,
    MksServo42D_485,
    MksServo42D_CAN,
    MksServo57D_485,
    MksServo57D_CAN,
    MksServo28D_485,
    MksServo28D_CAN,
    MksServo35D_485,
    MksServo35D_CAN
}

public enum PositionControlMode
{
    Relative,
    Absolute
}

public enum WorkMode : byte
{
    CrOpen = 0x00,
    CrClose = 0x01,
    CrVfoc = 0x02,
    SrOpen = 0x03,
    SrClose = 0x04,
    SrVfoc = 0x05
}

public enum AutoZeroStatus : byte
{
    Disable = 0x00,
    Enable = 0x01
}

public enum StallProtectStatus : byte
{
    Disable = 0x00,
    Enable = 0x01
}

public enum StateSwitch : byte
{
    Disable = 0x00,
    Enable = 0x01
}

public enum UartBaudRate : byte
{
    Baud9600 = 0x01,
    Baud19200 = 0x02,
    Baud25000 = 0x03,
    Baud38400 = 0x04,
    Baud57600 = 0x05,
    Baud115200 = 0x06,
    Baud256000 = 0x07
}

public enum LimitTriggerLevel : byte
{
    Low = 0x00,
    High = 0x01
}

public enum SingleTurnHomeMode : byte
{
    Disable = 0x00,
    DirMode = 0x01,
    NearMode = 0x02
}

public enum SingleTurnZeroSetting : byte
{
    Clear0 = 0x00,
    Set0 = 0x01,
    Hold0 = 0x02
}

public enum NoLimitSwitchHomeMode : byte
{
    LimitHome = 0x00,
    NoLimitHome = 0x01
}

public enum KeyLockStatus : byte
{
    Unlock = 0x00,
    Lock = 0x01
}
