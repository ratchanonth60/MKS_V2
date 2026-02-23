using System;
using System.Collections.Generic;
using RJCP.IO.Ports;
using MKS_V2.Core;

namespace MKS_V2.Services;

public class SerialService : ISerialService
{
    private SerialPortStream? _serialPort;
    private readonly List<byte> _rxBuffer = new List<byte>();

    public bool IsOpen => _serialPort?.IsOpen ?? false;

    public event EventHandler<byte[]>? DataReceived;
    public event EventHandler<string>? ErrorOccurred;

    public string[] GetAvailablePorts()
    {
        // SerialPortStream doesn't define GetPortNames(), fallback to System.IO.Ports for enumeration only
        return System.IO.Ports.SerialPort.GetPortNames();
    }

    public void Connect(string portName, int baudRate)
    {
        if (IsOpen) return;

        try
        {
            _serialPort = new SerialPortStream(portName, baudRate)
            {
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, $"Connection failed: {ex.Message}");
            Disconnect();
        }
    }

    private void SerialPort_DataReceived(object? sender, SerialDataReceivedEventArgs e)
    {
        if (_serialPort == null || !_serialPort.IsOpen) return;

        // Ensure there is actually data to read based on the event args type
        if (e.EventType == SerialData.Chars || e.EventType == SerialData.Eof)
        {
            try
            {
                int bytesToRead = _serialPort.BytesToRead;
                if (bytesToRead > 0)
                {
                    byte[] incoming = new byte[bytesToRead];
                    int actualRead = _serialPort.Read(incoming, 0, bytesToRead);

                    if (actualRead > 0)
                    {
                        if (actualRead < bytesToRead)
                        {
                            Array.Resize(ref incoming, actualRead);
                        }
                        _rxBuffer.AddRange(incoming);
                        ProcessBuffer();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Read error: {ex.Message}");
            }
        }
    }

    private void ProcessBuffer()
    {
        // Simple protocol framing based on first byte being FrameHeader (0xFA).
        // Since responses from MKS can be variable length depending on command, 
        // we might need more complex framing logic later.
        // For now we look for Header, then extract if we have enough bytes. 
        // We know standard query answers length base on Form1 or command type.
        // In this implementation we will pipe raw full frames to DataReceived.

        while (_rxBuffer.Count > 0)
        {
            if (_rxBuffer[0] != MksProtocolConstants.FrameHeader)
            {
                // Unrecognized starting byte, drop it
                _rxBuffer.RemoveAt(0);
                continue;
            }

            // A typical MKS packet has at least 4 bytes: Header + Addr + Cmd/Status + Checksum. 
            // In a complete implementation we parse Length. Since Form1 has a fixed set, we might
            // need to look ahead. As a simple fallback we search for the next FrameHeader or wait.
            // (A more robust solution requires a packet length dictionary based on CommandByte).

            // For MVP: Let's assume we read 5 bytes for an ack, or look for lengths.
            // As a stub for safe streaming, we will invoke DataReceived with whatever looks like a frame
            // and let MksResponseParser handle validation. If valid, we remove from buffer.

            // Wait until we have at least minimum packet size
            if (_rxBuffer.Count < 4) break;

            // For now, let's grab everything as a chunk.
            // A better way is finding expected length based on byte[2] (Command class).
            byte commandEcho = _rxBuffer[2];
            int expectedLength = GetExpectedResponseLength(commandEcho);

            if (_rxBuffer.Count < expectedLength)
            {
                break; // Wait for more data
            }

            var frame = _rxBuffer.GetRange(0, expectedLength).ToArray();
            _rxBuffer.RemoveRange(0, expectedLength);

            DataReceived?.Invoke(this, frame);
        }
    }

    private int GetExpectedResponseLength(byte commandEcho)
    {
        // Add specific lengths based on Form1.cs responses. 
        // Header(1) + Addr(1) + Cmd(1) + Payload(N) + Checksum(1) = 4 + N 
        switch (commandEcho)
        {
            case MksProtocolConstants.Commands.ReadEnableState: return 5;
            case MksProtocolConstants.Commands.ReadAccumulatedPulses: return 8; // payload 4
            case MksProtocolConstants.Commands.ReadPositionAngleError: return 6; // payload 2
            default: return 5; // fallback
        }
    }

    public void Disconnect()
    {
        if (_serialPort != null)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Disconnect error: {ex.Message}");
            }
            finally
            {
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }

    public void Write(byte[] buffer)
    {
        if (IsOpen && _serialPort != null)
        {
            try
            {
                _serialPort.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Write error: {ex.Message}");
            }
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
