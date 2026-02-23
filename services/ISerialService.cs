using System;
using System.IO.Ports;

namespace MKS_V2.Services;

public interface ISerialService : IDisposable
{
    bool IsOpen { get; }
    string[] GetAvailablePorts();
    void Connect(string portName, int baudRate);
    void Disconnect();
    void Write(byte[] buffer);

    /// <summary>
    /// Event triggered when a full MKS frame is received based on the header.
    /// This keeps the service decoupled from the parsing logic.
    /// </summary>
    event EventHandler<byte[]>? DataReceived;

    event EventHandler<string>? ErrorOccurred;
}
