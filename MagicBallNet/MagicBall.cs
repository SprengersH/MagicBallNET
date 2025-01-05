using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace MagicBallNet;

public class MagicBall : IDisposable
{
    private readonly SerialPort _serialPort;
    private readonly Encoding _encoding;
    private readonly int _interCharPauseMs = 50;

    public MagicBall(string port)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _encoding = Encoding.GetEncoding("IBM437");

        _serialPort = new SerialPort(port, 4800, Parity.Even, 8, StopBits.One)
        {
            ReadTimeout = 3000,
            WriteTimeout = 3000
        };
        _serialPort.Open();
    }

    public void Dispose()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public Dictionary<string, string> GetDeviceInfo()
    {
        WriteCommand(0x1B, 0x53, 0x03); // ESC S ETX
        byte[] rawData = ReadUntil(0x03);
        string[] parts = _encoding.GetString(rawData).Split('\0');

        var info = new Dictionary<string, string>();

        if (parts.Length > 0) info["Versie"] = parts[0];
        if (parts.Length > 1) info["Fabrikant"] = parts[1];
        if (parts.Length > 6) info["Serienummer"] = parts[6];
        if (parts.Length > 8) info["Standaard Tekst"] = $"{parts[7]} / {parts[8]}";
        if (parts.Length > 10) info["Font"] = $"{parts[9]} / {parts[10]}";
        if (parts.Length > 11) info["Geheugen"] = parts[11];

        return info;
    }


    public string GetStandardText()
    {
        WriteCommand(0x1B, 0x46, 0x03);
        byte[] rawData = ReadUntil(0x03);
        return DecodeText(rawData);
    }

    public string GetDisplayedText()
    {
        WriteCommand(0x1B, 0x54, 0x03);
        byte[] rawData = ReadUntil(0x03);
        return DecodeText(rawData);
    }

    public void SendFormattedText(string? text, int extraWhitespace = 10)
    {
        if (string.IsNullOrEmpty(text))
        {
            Console.WriteLine("[FOUT] Tekst mag niet leeg zijn.");
            return;
        }

        // Voeg extra whitespace toe
        string paddedText = text + new string(' ', extraWhitespace);

        WriteCommand(0x02, 0x0D); // STX + carriage return
        foreach (byte b in _encoding.GetBytes(paddedText))
        {
            WriteByteWithoutEcho(b);
        }
        WriteByteWithoutEcho(0x03); // ETX
    }


    public Dictionary<byte, string> ExploreMemoryPages()
    {
        var results = new Dictionary<byte, string>();
        for (byte page = 0x00; page <= 0x0F; page++)
        {
            try
            {
                WriteCommandWithoutEcho(0x1B, 0x54, page, 0x03); // ESC T <page> ETX
                Thread.Sleep(500);
                byte[] rawData = ReadUntil(0x03);
                string decoded = DecodeText(rawData);

                results[page] = string.IsNullOrEmpty(decoded) ? "[Leeg]" : decoded;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FOUT] Kan pagina {page:X2} niet lezen: {ex.Message}");
                results[page] = "[Geen respons]";
            }
        }
        return results;
    }


    public void MemoryMap()
    {
        ushort startAddress = 0x0000;
        ushort endAddress = 0x07EF;
        int batchSize = 16;

        Console.WriteLine("Geheugenoverzicht (Adres: Data):");
        while (startAddress <= endAddress)
        {
            for (ushort address = startAddress; address < startAddress + batchSize && address <= endAddress; address++)
            {
                try
                {
                    WriteCommandWithoutEcho(0x1B, 0x47, (byte)(address >> 8), (byte)(address & 0xFF), 0x03);
                    Thread.Sleep(50);
                    byte[] rawData = ReadUntil(0x03);
                    Console.WriteLine($"Adres {address:X4}: {DecodeText(rawData)}");
                }
                catch
                {
                    Console.WriteLine($"Adres {address:X4}: [Geen Data]");
                }
            }
            startAddress += (ushort)batchSize;
        }
    }

    public void SearchMemory(string searchTerm)
    {
        ushort startAddress = 0x0000;
        ushort endAddress = 0x07EF;

        Console.WriteLine($"Zoeken naar '{searchTerm}' in geheugen...");
        while (startAddress <= endAddress)
        {
            try
            {
                WriteCommandWithoutEcho(0x1B, 0x47, (byte)(startAddress >> 8), (byte)(startAddress & 0xFF), 0x03);
                Thread.Sleep(50);
                byte[] rawData = ReadUntil(0x03);
                string data = DecodeText(rawData);
                if (data.Contains(searchTerm))
                {
                    Console.WriteLine($"[Gevonden] Adres {startAddress:X4}: {data}");
                }
            }
            catch
            {
                // Geen data
            }
            startAddress++;
        }
    }

    public void CompareStandardTextWithMemory()
    {
        string standardText = GetStandardText();
        var memoryPages = ExploreMemoryPages();
        foreach (var kvp in memoryPages)
        {
            Console.WriteLine(kvp.Value.Contains(standardText)
                ? $"[Match] Standaardtekst in pagina {kvp.Key:X2}"
                : $"[Geen Match] Pagina {kvp.Key:X2}");
        }
    }

    public void ResetDevice()
    {
        WriteCommandWithoutEcho(0x1B, 0x52, 0x03);
        Thread.Sleep(1000);
    }

    private void WriteCommand(params byte[] commandBytes)
    {
        foreach (byte b in commandBytes)
        {
            WriteByteWithoutEcho(b);
        }
    }

    private void WriteCommandWithoutEcho(params byte[] commandBytes)
    {
        foreach (byte b in commandBytes)
        {
            _serialPort.Write(new[] { b }, 0, 1);
            Thread.Sleep(_interCharPauseMs);
        }
    }

    private byte[] ReadUntil(byte lastByte)
    {
        var buffer = new List<byte>();
        try
        {
            while (true)
            {
                int b = _serialPort.ReadByte();
                buffer.Add((byte)b);
                if (b == lastByte) break;
            }
        }
        catch (TimeoutException)
        {
            Console.WriteLine("[FOUT] Tijdslimiet bereikt tijdens het lezen.");
        }

        return buffer.ToArray();
    }


    private string DecodeText(byte[] rawData)
    {
        if (rawData == null || rawData.Length == 0)
        {
            return "[Geen data ontvangen]";
        }

        return _encoding.GetString(rawData).TrimEnd('\0');
    }


    private void WriteByteWithoutEcho(byte value)
    {
        _serialPort.Write(new[] { value }, 0, 1);
        Thread.Sleep(_interCharPauseMs);
    }
}
