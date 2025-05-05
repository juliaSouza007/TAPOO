using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static readonly List<string> criptomoedas = new List<string>
    {
        "BTC",  // Bitcoin
        "ETH",  // Ethereum
        "LTC",  // Litecoin
        "BCH",  // Bitcoin Cash
        "XRP",  // Ripple
        "ADA",  // Cardano
        "DOT",  // Polkadot
        "LINK", // Chainlink
        "XLM",  // Stellar
        "DOGE"  // Dogecoin
    };

    static readonly ConcurrentDictionary<string, decimal> PrecosAnteriores = new();

    static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        _ = MonitorarTeclaEscAsync(cts);

        while (!cts.Token.IsCancellationRequested)
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Atualização: {DateTime.Now:HH:mm:ss}\n");

                var tarefas = new List<Task>();

                foreach (var simbolo in criptomoedas)
                {
                    tarefas.Add(ProcessarCotacaoAsync(simbolo, cts.Token));
                }

                await Task.WhenAll(tarefas);

                Console.WriteLine("\n(Pressione ESC para sair)");

                await Task.Delay(TimeSpan.FromSeconds(30), cts.Token);
            }
            catch (TaskCanceledException)
            {
                // ESC pressionado, finaliza o programa.
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
            }
        }
    }

    static HttpClient CriarClienteHttp()
    {
        var cliente = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        cliente.DefaultRequestHeaders.Add("User-Agent", "MonitorCripto/1.0");
        cliente.DefaultRequestHeaders.Add("Accept", "application/json");
        return cliente;
    }

    static async Task ProcessarCotacaoAsync(string simbolo, CancellationToken token)
    {
        try
        {
            var clienteHttp = CriarClienteHttp();
            var url = $"https://api.exchange.cryptomkt.com/api/3/public/price/rate?from={simbolo}&to=USDT";

            var resposta = await clienteHttp.GetAsync(url, token);
            resposta.EnsureSuccessStatusCode();
            var json = await resposta.Content.ReadAsStringAsync(token);

            using var documento = JsonDocument.Parse(json);
            if (documento.RootElement.TryGetProperty(simbolo, out var dadosMoeda))
            {
                var precoString = dadosMoeda.GetProperty("price").GetString();
                if (!string.IsNullOrEmpty(precoString))
                {
                    var precoAtual = decimal.Parse(precoString, CultureInfo.InvariantCulture);

                    PrecosAnteriores.TryGetValue(simbolo, out var precoAnterior);
                    PrecosAnteriores[simbolo] = precoAtual;

                    ExibirResultadosNoConsole(simbolo, precoAtual, precoAnterior);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{simbolo}: Erro ao obter cotação ({ex.Message})");
            Console.ResetColor();
        }
    }

    static void ExibirResultadosNoConsole(string simbolo, decimal precoAtual, decimal precoAnterior)
    {
        var subiu = precoAtual > precoAnterior;
        var setinha = precoAtual == precoAnterior ? "-" : subiu ? "↑" : "↓";

        // ANSI para PowerShell e CMD modernos
        var cor = precoAnterior == 0 ? "\u001b[37m" : subiu ? "\u001b[32m" : "\u001b[31m";

        Console.WriteLine($"{cor}{simbolo}: ${precoAtual:N4} {setinha}\u001b[0m");
    }

    static async Task MonitorarTeclaEscAsync(CancellationTokenSource cts)
    {
        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                cts.Cancel();
                break;
            }
            await Task.Delay(100);
        }
    }
}