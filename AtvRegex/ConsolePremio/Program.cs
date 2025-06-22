using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string json = File.ReadAllText("prize.json");

        string pattern = @"""category""\s*:\s*""economics""[\s\S]*?""firstname""\s*:\s*""([^""]+)""";
        Match match = Regex.Match(json, pattern);

        Console.WriteLine("\n-------------------------");
        Console.WriteLine("Lendo o arquivo JSON...");
        Console.WriteLine("-------------------------\n");

        if (match.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Primeiro ganhador de economia: {match.Groups[1].Value}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Nenhum ganhador de economia encontrado.");
            Console.ResetColor();
        }
    }
}
