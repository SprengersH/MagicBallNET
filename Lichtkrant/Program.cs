using Lichtkrant;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("== Lichtkrant CLI ==");

        string port = "COM6"; // Pas aan naar de juiste COM-poort
        using var magicBall = new MagicBall(port);

        while (true)
        {
            Console.WriteLine("\nKies een optie:");
            Console.WriteLine("1. Apparaat Informatie Ophalen");
            Console.WriteLine("2. Lees Standaard Tekst");
            Console.WriteLine("3. Lees Huidige Tekst");
            Console.WriteLine("4. Verstuur Nieuwe Tekst");
            Console.WriteLine("5. Onderzoek Geheugenpagina's");
            Console.WriteLine("6. Geheugenkaart Weergeven");
            Console.WriteLine("7. Zoek in Geheugen");
            Console.WriteLine("8. Vergelijk Standaardtekst met Geheugenpagina's");
            Console.WriteLine("9. Reset Apparaat");
            Console.WriteLine("10. Stoppen");

            Console.Write("Uw keuze: ");
            string? keuze = Console.ReadLine();

            try
            {
                switch (keuze)
                {
                    case "1":
                        var info = magicBall.GetDeviceInfo();
                        Console.WriteLine("Apparaat Informatie:");
                        foreach (var kv in info)
                        {
                            Console.WriteLine($"{kv.Key}: {kv.Value}");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Standaard Tekst:");
                        Console.WriteLine(magicBall.GetStandardText());
                        break;

                    case "3":
                        Console.WriteLine("Huidige Tekst op de Lichtkrant:");
                        Console.WriteLine(magicBall.GetDisplayedText());
                        break;

                    case "4":
                        Console.Write("Voer de tekst in die je wilt weergeven: ");
                        string? text = Console.ReadLine();

                        Console.Write("Voer het aantal extra spaties in (standaard: 10): ");
                        if (!int.TryParse(Console.ReadLine(), out int extraWhitespace))
                        {
                            extraWhitespace = 10; // Gebruik standaardwaarde
                        }

                        magicBall.SendFormattedText(text, extraWhitespace);
                        Console.WriteLine("Tekst verstuurd!");
                        break;


                    case "5":
                        Console.WriteLine("Onderzoek van geheugenpagina's...");
                        var memoryPages = magicBall.ExploreMemoryPages();
                        foreach (var kvp in memoryPages)
                        {
                            Console.WriteLine($"Pagina {kvp.Key:X2}: {kvp.Value}");
                        }
                        break;

                    case "6":
                        Console.WriteLine("Geheugenkaart:");
                        magicBall.MemoryMap();
                        break;

                    case "7":
                        Console.Write("Voer de tekst in om te zoeken: ");
                        string? searchTerm = Console.ReadLine();
                        magicBall.SearchMemory(searchTerm ?? string.Empty);
                        break;

                    case "8":
                        Console.WriteLine("Vergelijk Standaardtekst met Geheugenpagina's...");
                        magicBall.CompareStandardTextWithMemory();
                        break;

                    case "9":
                        Console.WriteLine("Reset het apparaat...");
                        magicBall.ResetDevice();
                        Console.WriteLine("Reset voltooid!");
                        break;

                    case "10":
                        Console.WriteLine("Programma beëindigd.");
                        return;

                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FOUT] {ex.Message}");
            }
        }
    }
}
