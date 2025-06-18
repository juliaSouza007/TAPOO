using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    static void Main()
    {
        string json = File.ReadAllText("prize.json"); 
        JsonNode root = JsonNode.Parse(json);

        foreach (JsonNode prize in root["prizes"].AsArray())
        {
            if (prize["category"]?.ToString() == "economics")
            {
                var laureates = prize["laureates"].AsArray();
                if (laureates.Count > 0)
                {
                    string firstname = laureates[0]["firstname"]?.ToString();

                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Lendo o arquivo JSON...");
                    Console.WriteLine("-------------------------\n");

                    Console.WriteLine($"Categoria: {prize["category"]}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Primeiro ganhador de economia: {firstname}");
                    Console.ResetColor();
                    break;
                }
            }
        }
    }
}
