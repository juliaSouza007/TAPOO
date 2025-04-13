using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Versao3_5
{
    static BlockingCollection<(int pedido, int prato)> pedidos = new();
    static object lockConsole = new();

    static int arroz = 0, carne = 0, macarrao = 0, molho = 0;
    static int pedidoId = 0;

    static void ConsoleLock(string msg, ConsoleColor color)
    {
        lock (lockConsole)
        {
            var aux = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = aux;
        }
    }

    static void Garcom()
    {
        var rnd = new Random();
        var id = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"[Garçom {id}] Pronto para receber pedidos!");
        while (true)
        {
            Thread.Sleep(rnd.Next(1000, 10000));
            int prato = rnd.Next(1, 4);
            int idPedido = Interlocked.Increment(ref pedidoId);

            ConsoleLock($"[Garçom] - Envio de Pedido {idPedido}: Prato {prato}", ConsoleColor.Blue);
            pedidos.Add((idPedido, prato));
        }
    }

    static void Produzir(string item, int quantidade)
    {
        ConsoleLock($"[Chef] Iniciando produção de {item}", ConsoleColor.Green);
        Thread.Sleep(2000);
        switch (item)
        {
            case "arroz": arroz += quantidade; break;
            case "carne": carne += quantidade; break;
            case "macarrao": macarrao += quantidade; break;
            case "molho": molho += quantidade; break;
        }
        ConsoleLock($"[Chef] Finalizou produção de {item}. Estoque atualizado: {GetEstoque(item)} unidades", ConsoleColor.Green);
    }

    static int GetEstoque(string item) => item switch
    {
        "arroz" => arroz,
        "carne" => carne,
        "macarrao" => macarrao,
        "molho" => molho,
        _ => 0
    };

    static void Consumir(string item, int quantidade)
    {
        switch (item)
        {
            case "arroz": arroz -= quantidade; break;
            case "carne": carne -= quantidade; break;
            case "macarrao": macarrao -= quantidade; break;
            case "molho": molho -= quantidade; break;
        }
    }

    static void Chef()
    {
        Console.WriteLine($"[Chef {Thread.CurrentThread.ManagedThreadId}] Pronto para preparar pedidos!");
        foreach (var (idPedido, prato) in pedidos.GetConsumingEnumerable())
        {
            ConsoleLock($"[Chef] Inicio da Preparação do Pedido {idPedido}", ConsoleColor.Red);

            switch (prato)
            {
                case 1:
                    if (arroz < 1) Produzir("arroz", 3);
                    if (carne < 1) Produzir("carne", 2);
                    Consumir("arroz", 1);
                    Consumir("carne", 1);
                    Thread.Sleep(2000);
                    break;
                case 2:
                    if (macarrao < 1) Produzir("macarrao", 4);
                    if (molho < 1) Produzir("molho", 2);
                    Consumir("macarrao", 1);
                    Consumir("molho", 1);
                    Thread.Sleep(2000);
                    break;
                case 3:
                    if (arroz < 1) Produzir("arroz", 3);
                    if (carne < 1) Produzir("carne", 2);
                    if (molho < 1) Produzir("molho", 2);
                    Consumir("arroz", 1);
                    Consumir("carne", 1);
                    Consumir("molho", 1);
                    Thread.Sleep(3000);
                    break;
            }

            ConsoleLock($"[Chef] Fim da Preparação do Pedido {idPedido}", ConsoleColor.Red);
        }
    }

    public static void Iniciar()
    {
        for (int i = 0; i < 5; i++) Task.Run(Garcom);
        for (int i = 0; i < 3; i++) Task.Run(Chef);
    }
}