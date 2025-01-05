using System;
using System.IO.Ports;
using System.Text;
using System.IO;

namespace LichtkrantInteractief
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("== Lichtkrant Interactief Testen ==");

            // Seriële instellingen
            string portName = "COM6";
            int baudRate = 4800;
            Parity parity = Parity.None;
            int dataBits = 8;
            StopBits stopBits = StopBits.Two;

            // Logging bestand
            string logFile = "LichtkrantInteractiefLog.txt";

            // Testconfiguratie
            int startCmd = 0x00; // Begin bij 0x00
            int endCmd = 0xFF;   // Tot en met 0xFF
            string[] testTexts = { "", "Test", "1234567890" }; // Testteksten

            using var port = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
            {
                Handshake = Handshake.None,
                DtrEnable = false,
                RtsEnable = true,
                Encoding = Encoding.ASCII,
                ReadTimeout = 1500,
                WriteTimeout = 1500
            };

            try
            {
                port.Open();
                Console.WriteLine($"[OK] Poort {port.PortName} geopend @ {baudRate} baud, 8{parity}, stop={stopBits}.");

                using var writer = new StreamWriter(logFile, append: false) { AutoFlush = true };
                writer.WriteLine("Command;Text;ResponseHEX;ResponseASCII;ObservedEffect");

                // Loop door alle commandobytes
                for (int cmd = startCmd; cmd <= endCmd; cmd++)
                {
                    foreach (var text in testTexts)
                    {
                        // 1. Scherm wissen voor elke test
                        ClearScreen(port);

                        // 2. Commando verzenden
                        byte[] packet = BuildPacket((byte)cmd, text);
                        Console.WriteLine($"\n[Test] CMD=0x{cmd:X2}, TEXT='{text}'");
                        Console.WriteLine($"[Verzenden] HEX={BitConverter.ToString(packet)}");
                        port.Write(packet, 0, packet.Length);

                        // 3. Wachten en respons uitlezen
                        Thread.Sleep(500);
                        string responseHex, responseAscii;
                        ReadResponse(port, out responseHex, out responseAscii);

                        // 4. Toon de respons
                        Console.WriteLine($"[Response HEX] {responseHex}");
                        Console.WriteLine($"[Response ASCII] {responseAscii}");

                        // 5. Vraag om observatie
                        Console.WriteLine("Kijk op de lichtkrant. Wat zie je? Voer je observatie in (of druk Enter om niets te noteren):");
                        string observation = Console.ReadLine();

                        // 6. Loggen van het resultaat
                        writer.WriteLine($"{cmd:X2};{text};{responseHex};{responseAscii};{observation}");

                        Console.WriteLine("Ga verder met Enter...");
                        Console.ReadLine();
                    }
                }

                Console.WriteLine($"Tests afgerond. Resultaten opgeslagen in: {logFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FOUT] {ex.Message}");
            }
            finally
            {
                if (port.IsOpen)
                {
                    port.Close();
                    Console.WriteLine("[GESLOTEN] Poort");
                }
            }

            Console.WriteLine("== Klaar. Druk op Enter om af te sluiten. ==");
            Console.ReadLine();
        }

        /// <summary>
        /// Stuurt een clear-commando naar de lichtkrant.
        /// </summary>
        private static void ClearScreen(SerialPort port)
        {
            byte[] clearPacket = { 0x02, 0x05, 0x03 }; // STX 0x05 ETX
            port.Write(clearPacket, 0, clearPacket.Length);
            Thread.Sleep(500); // Even wachten tot het scherm gewist is
            Console.WriteLine("[Scherm gewist]");
        }

        /// <summary>
        /// Bouwt een pakket: STX, CMD, [TEXT], ETX
        /// </summary>
        private static byte[] BuildPacket(byte command, string text)
        {
            byte[] textBytes = string.IsNullOrEmpty(text) ? Array.Empty<byte>() : Encoding.ASCII.GetBytes(text);
            byte[] packet = new byte[3 + textBytes.Length];
            packet[0] = 0x02; // STX
            packet[1] = command;
            Array.Copy(textBytes, 0, packet, 2, textBytes.Length);
            packet[2 + textBytes.Length] = 0x03; // ETX
            return packet;
        }

        /// <summary>
        /// Leest de respons van de lichtkrant in HEX en ASCII.
        /// </summary>
        private static void ReadResponse(SerialPort port, out string responseHex, out string responseAscii)
        {
            if (port.BytesToRead > 0)
            {
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);
                responseHex = BitConverter.ToString(buffer);
                responseAscii = port.Encoding.GetString(buffer);
            }
            else
            {
                responseHex = "[Geen data ontvangen]";
                responseAscii = "[Geen data ontvangen]";
            }
        }
    }
}
