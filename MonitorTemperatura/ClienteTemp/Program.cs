using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Bem-vindo ao monitor de temperatura!\n");
        Console.WriteLine("Digite Ctrl + C para sair :)");

        bool continuar = true;

        // Captura Ctrl+C para encerrar o programa
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;  // cancela o encerramento abrupto do console
            continuar = false;
            Console.WriteLine("\nEncerrando monitoramento de temperatura.");
        };

        string unidade = SolicitarUnidadeTemperatura();
        int intervaloSegundos = SolicitarIntervalo();

        double temperaturaAnterior = 0.0;
        DateTime horarioAnterior = DateTime.MinValue;

        while (continuar)
        {
            try
            {
                var temperaturaAtual = await ObterTemperaturaAsync(unidade);
                DateTime horarioAtual = DateTime.Now;
                string variacao = CompararTemperaturas(temperaturaAnterior, temperaturaAtual);
                ImprimirResultado(horarioAtual, temperaturaAtual, variacao);
                temperaturaAnterior = temperaturaAtual;
                horarioAnterior = horarioAtual;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Erro ao obter leitura: {ex.Message}");
                Console.ResetColor();
            }

            // Espera intervalo, mas se for interrompido sai antes
            int totalMs = intervaloSegundos * 1000;
            int waited = 0;
            int waitStep = 100; // checar a cada 100ms

            while (continuar && waited < totalMs)
            {
                await Task.Delay(waitStep);
                waited += waitStep;
            }
        }
    }

    static string SolicitarUnidadeTemperatura()
    {
        Console.WriteLine("Escolha a unidade de temperatura desejada:");
        Console.WriteLine("1. Celsius");
        Console.WriteLine("2. Kelvin");
        Console.WriteLine("3. Fahrenheit");

        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
        } while (key.KeyChar != '1' && key.KeyChar != '2' && key.KeyChar != '3');

        Console.WriteLine();

        switch (key.KeyChar)
        {
            case '1':
                return "celsius";
            case '2':
                return "kelvin";
            case '3':
                return "fahrenheit";
            default:
                return "celsius"; // Opção padrão caso algo dê errado
        }
    }

    static int SolicitarIntervalo()
    {
        int intervalo;
        while (true)
        {
            Console.Write("Digite o intervalo em segundos para cada nova leitura: ");
            if (int.TryParse(Console.ReadLine(), out intervalo) && intervalo > 0)
                return intervalo;
            else
                Console.WriteLine("Intervalo inválido. Digite um valor positivo.");
        }
    }

    static async Task<double> ObterTemperaturaAsync(string unidade)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync($"http://localhost:5053/temperatura/{unidade}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var temperatura = JsonSerializer.Deserialize<TemperaturaResponse>(content);
                return temperatura.Valor;
            }
            else
            {
                throw new HttpRequestException($"Erro na requisição: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }

    static string CompararTemperaturas(double anterior, double atual)
    {
        if (anterior == 0.0)
            return ""; // Primeira leitura, não comparação necessária

        if (atual > anterior)
            return "SUBIU";
        else if (atual < anterior)
            return "DESCEU";
        else
            return "SEM ALTERAÇÃO";
    }

    static void ImprimirResultado(DateTime horario, double temperatura, string variacao)
    {
        string horarioFormatado = horario.ToString("HH:mm:ss");
        string temperaturaFormatada = $"{temperatura:F2}";

        switch (variacao)
        {
            case "SUBIU":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "DESCEU":
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            default:
                Console.ResetColor();
                break;
        }

        Console.WriteLine($"[{horarioFormatado}] Temperatura: {temperaturaFormatada} °C → {variacao}");
        Console.ResetColor();
    }

    class TemperaturaResponse
    {
        [JsonPropertyName("unidade")]
        public string Unidade { get; set; }
        [JsonPropertyName("valor")]
        public double Valor { get; set; }
    }
}