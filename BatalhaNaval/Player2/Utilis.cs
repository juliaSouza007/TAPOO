using System;

namespace Player2
{
    public static class Utils
    {
        public static (int, int) ParseCoordinate(string coord)
        {
            coord = coord.ToUpper();
            int row = coord[0] - 'A';
            int col = int.Parse(coord.Substring(1));
            return (row, col);
        }

        public static bool IsValidCoordinate(string coord)
        {
            if (coord.Length < 2 || coord.Length > 3)
                return false;

            char row = char.ToUpper(coord[0]);
            if (row < 'A' || row > 'J')
                return false;

            if (!int.TryParse(coord.Substring(1), out int col))
                return false;

            return col >= 0 && col < 10;
        }
    }
}