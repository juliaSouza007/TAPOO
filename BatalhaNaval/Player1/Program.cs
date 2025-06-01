using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Player1
{
    class Program
    {
        static Board board = new Board();
        static char[,] opponentView = new char[10, 10]; // Histórico dos ataques no oponente
        static NetworkStream stream;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Olá Pirata! Bem-vindo(a) ao <<BATALHA NAVAL>>");
            Console.ResetColor();
            Console.WriteLine("Escolha o modo de posicionamento:");
            Console.WriteLine("1 - Aleatório");
            Console.WriteLine("2 - Manual");
            Console.Write("Opção: ");
            string option = Console.ReadLine();

            if (option == "1")
                board.PlaceShipsRandomly(10);
            else
                board.PlaceShipsManually(10);

            InitializeOpponentView();

            StartServer(5000);

            while (true)
            {
                // 1 - Player1 ataca primeiro
                while (true)
                {
                    Console.Write("Sua vez de atacar (ex: B7): ");
                    string attackCoord = Console.ReadLine().ToUpper();

                    try
                    {
                        (int ar, int ac) = ParseCoord(attackCoord);

                        if (ar < 0 || ar >= 10 || ac < 0 || ac >= 10)
                        {
                            Console.WriteLine("Coordenada fora do tabuleiro. Tente novamente.");
                            continue;
                        }

                        if (opponentView[ar, ac] == 'X' || opponentView[ar, ac] == 'O')
                        {
                            Console.WriteLine("Você já atacou essa posição! Escolha outra.");
                            continue;
                        }

                        Send(attackCoord);
                        string attackResponse = Receive();

                        if (attackResponse == "HIT" || attackResponse == "WIN")
                            opponentView[ar, ac] = 'X';
                        else
                            opponentView[ar, ac] = 'O';

                        PrintBoardsSideBySide(board, opponentView);

                        if (attackResponse == "WIN")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Você venceu!");
                            Console.ResetColor();
                            break;
                        }

                        break;  // ataque foi feito com sucesso
                    }
                    catch
                    {
                        Console.WriteLine("Entrada inválida. Use o formato correto (ex: B7).");
                    }
                }

                // 2 - Receber ataque do Player2
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Esperando o Player 2 atacar...");
                Console.ResetColor();
                string enemyAttack = Receive();
                (int r, int c) = ParseCoord(enemyAttack);

                string response = "";

                if (board.IsShip(r, c))
                {
                    board.MarkHit(r, c);
                    response = board.AreAllShipsSunk() ? "WIN" : "HIT";
                }
                else
                {
                    board.MarkMiss(r, c);
                    response = "MISS";
                }

                Send(response);

                PrintBoardsSideBySide(board, opponentView);

                if (response == "WIN")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Você perdeu!");
                    Console.ResetColor();
                    break;
                }
            }

            stream.Close();
        }

        static void InitializeOpponentView()
        {
            for (int r = 0; r < 10; r++)
                for (int c = 0; c < 10; c++)
                    opponentView[r, c] = '~';
        }

        static void PrintBoardsSideBySide(Board board, char[,] opponentView)
        {
            Console.WriteLine("Meu mapa".PadRight(27) + "Mapa do oponente");
            Console.Write("   ");
            for (int c = 0; c < 10; c++) Console.Write($"{c} ");
            Console.Write("    ");
            Console.Write("   ");
            for (int c = 0; c < 10; c++) Console.Write($"{c} ");
            Console.WriteLine();

            for (int r = 0; r < 10; r++)
            {
                // Imprime "Meu mapa"
                Console.Write($"{(char)('A' + r)}  ");
                for (int c = 0; c < 10; c++)
                {
                    PrintCell(board.GetCell(r, c), showShips: true);
                }

                Console.Write("    "); // Espaço entre os tabuleiros

                // Imprime "Mapa do oponente"
                Console.Write($"{(char)('A' + r)}  ");
                for (int c = 0; c < 10; c++)
                {
                    PrintCell(opponentView[r, c], showShips: false);
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }

        static void PrintCell(char cell, bool showShips)
        {
            if (cell == 'X')
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X ");
            }
            else if (cell == 'O')
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("O ");
            }
            else if (cell == '*' && showShips)
            {
                // Mostra os navios como X em vermelho no próprio tabuleiro
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("X ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("~ ");
            }
            Console.ResetColor();
        }

        static void StartServer(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Aguardando Player2 na porta {port}...");
            var client = listener.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("Player2 conectado!");
        }

        static void Send(string msg)
        {
            var data = Encoding.ASCII.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }

        static string Receive()
        {
            var buf = new byte[32];
            int len = stream.Read(buf, 0, buf.Length);
            return Encoding.ASCII.GetString(buf, 0, len);
        }

        static (int, int) ParseCoord(string coord)
        {
            coord = coord.ToUpper();
            int r = coord[0] - 'A';
            int c = int.Parse(coord.Substring(1));
            return (r, c);
        }
    }
}
