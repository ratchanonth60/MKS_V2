using System;
using System.Buffers.Binary;

namespace MKS_V2.Core;

/// <summary>
/// A class representing parsed data from a motor response 
/// (E.g. from ReadAllStatus or ReadAllParams)
/// </summary>
public class MksResponseResult
{
    public bool IsValid { get; set; }
    public byte CommandEcho { get; set; }

    // Extracted payload
    public byte[]? Payload { get; set; }

    // Helpers to cast payload for common responses
    public int? AsInt32()
    {
        if (Payload?.Length == 4) return BinaryPrimitives.ReadInt32BigEndian(Payload);
        return null; // or little endian depending on the original implementation
    }

    public ushort? AsUInt16()
    {
        if (Payload?.Length == 2) return BinaryPrimitives.ReadUInt16BigEndian(Payload);
        return null;
    }
}

public static class MksResponseParser
{
    private static byte CalculateChecksum(ReadOnlySpan<byte> buffer)
    {
        int sum = 0;
        for (int i = 0; i < buffer.Length - 1; i++)
        {
            sum += buffer[i];
        }
        return (byte)(sum & 0xFF);
    }

    /// <summary>
    /// Validates an incoming buffer based on MKS protocol Header and Checksum
    /// </summary>
    public static MksResponseResult Parse(ReadOnlySpan<byte> responseBuffer)
    {
        var result = new MksResponseResult();

        if (responseBuffer.Length < 4)
            return result; // Invalid, too short

        if (responseBuffer[0] != MksProtocolConstants.FrameHeader)
            return result; // Invalid Header

        byte expectedChecksum = CalculateChecksum(responseBuffer);
        byte actualChecksum = responseBuffer[^1];

        if (expectedChecksum != actualChecksum)
            return result; // Invalid Checksum

        result.IsValid = true;
        result.CommandEcho = responseBuffer[2]; // Commnand that is being responded to

        if (responseBuffer.Length > 4)
        {
            result.Payload = responseBuffer[3..^1].ToArray();
        }

        return result;
    }
}
