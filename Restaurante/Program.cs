using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Escolha a versão da simulação:");
        Console.WriteLine("1 - Versão com 1 Chef e 1 Garçom");
        Console.WriteLine("2 - Versão com 3 Chefs e 5 Garçons");
        Console.Write("Opção: ");

        var opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                Versao1_1.Iniciar();
                break;
            case "2":
                Versao3_5.Iniciar();
                break;
            default:
                Console.WriteLine("Opção inválida. Encerrando o programa.");
                return;
        }

        Console.WriteLine("Pressione Enter para encerrar...");
        Console.ReadLine();
    }
}