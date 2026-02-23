namespace MKS_V2.Models;

public class ConnectionConfig
{
    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 115200;
    public byte DeviceAddress { get; set; } = 0x01;
}
