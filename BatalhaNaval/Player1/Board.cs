class Board
{
    private char[,] board = new char[10, 10];

    public Board()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int r = 0; r < 10; r++)
            for (int c = 0; c < 10; c++)
                board[r, c] = '~';
    }

    public void PlaceShipsRandomly(int n)
    {
        var rand = new Random();
        int placed = 0;
        while (placed < n)
        {
            int r = rand.Next(10);
            int c = rand.Next(10);
            if (board[r, c] == '~')
            {
                board[r, c] = '*';
                placed++;
            }
        }
    }

    public void PlaceShipsManually(int n)
    {
        int placed = 0;
        while (placed < n)
        {
            Console.Write($"Digite a coordenada do navio {placed + 1} (ex: A5): ");
            string coord = Console.ReadLine().ToUpper();

            try
            {
                int r = coord[0] - 'A';
                int c = int.Parse(coord.Substring(1));
                if (r >= 0 && r < 10 && c >= 0 && c < 10 && board[r, c] == '~')
                {
                    board[r, c] = '*';
                    placed++;
                }
                else
                {
                    Console.WriteLine("Coordenada inválida ou já ocupada. Tente novamente.");
                }
            }
            catch
            {
                Console.WriteLine("Entrada inválida. Use o formato correto (ex: A5).");
            }
        }
    }

    public bool IsShip(int r, int c)
    {
        return board[r, c] == '*';
    }

    public void MarkHit(int r, int c)
    {
        board[r, c] = 'X';
    }

    public void MarkMiss(int r, int c)
    {
        if (board[r, c] == '~')
            board[r, c] = 'O';
    }

    public bool AreAllShipsSunk()
    {
        for (int r = 0; r < 10; r++)
            for (int c = 0; c < 10; c++)
                if (board[r, c] == '*')
                    return false;
        return true;
    }

    public char GetCell(int r, int c)
    {
        return board[r, c];
    }
}