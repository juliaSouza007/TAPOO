using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()+=_\-{}\[\]:;""'?<>,.]).{7,16}$";

        while (true)
        {
            Console.Write("Digite uma senha: ");
            string senha = Console.ReadLine();

            bool temMinuscula = Regex.IsMatch(senha, "[a-z]");
            bool temMaiuscula = Regex.IsMatch(senha, "[A-Z]");
            bool temNumero = Regex.IsMatch(senha, @"\d");
            bool temEspecial = Regex.IsMatch(senha, @"[!@#$%^&*()+=_\-{}\[\]:;""'?<>,.]");
            bool tamanhoCorreto = senha.Length >= 7 && senha.Length <= 16;

            Console.WriteLine("\nVerificando requisitos:\n");

            MostrarRequisito("Entre 7 e 16 caracteres", tamanhoCorreto);
            MostrarRequisito("Pelo menos uma letra minúscula", temMinuscula);
            MostrarRequisito("Pelo menos uma letra maiúscula", temMaiuscula);
            MostrarRequisito("Pelo menos um número", temNumero);
            MostrarRequisito("Pelo menos um caractere especial permitido", temEspecial);

            if (Regex.IsMatch(senha, pattern))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔️ Senha forte! Programa finalizado.");
                Console.ResetColor();
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Senha fraca! Tente novamente.\n");
                Console.ResetColor();
            }
        }
    }

    static void MostrarRequisito(string texto, bool atende)
    {
        Console.ForegroundColor = atende ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"{texto}");
        Console.ResetColor();
    }
}
