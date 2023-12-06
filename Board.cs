namespace Battleship
{
    class Board
    {
        public string[,] cells = new string[10, 10];
        public string Name;
        public bool Fancy;
        public Board(string name, bool fancy)
        {
            Name = name;
            Fancy = fancy;
            CreateBoardCells();
        }
        public void CreateBoardCells()
        {
            string emptyCell;
            if (Fancy) { emptyCell = " "; }
            else { emptyCell = "■"; }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    cells[i, j] = emptyCell;
                }
            }
        }
        public void RandomCell(int orietation, int boatSize, out int row, out int col)
        {
            Random coordinates = new();
            if (orietation == 0) // horizontal
            {
                row = coordinates.Next(10);
                col = coordinates.Next(10 - boatSize + 1);
            }
            else // vertical
            {
                row = coordinates.Next(10 - boatSize + 1);
                col = coordinates.Next(10);
            }
        }
        public void PlaceShips()
        {
            string emptyCell;
            if (Fancy) { emptyCell = " "; }
            else { emptyCell = "■"; }
            Random orientation = new();
            for (int i = 0; i < Ship.ships.Count; i++) // FOR EVERY SHIP
            {
                int orientationNum = orientation.Next(2);
                RandomCell(orientationNum, Ship.ships[i].Size, out int row, out int col); // PICK RANDOM CELL
                bool[] shipCells = new bool[Ship.ships[i].Size];
                for (int j = 0; j < Ship.ships[i].Size; j++) // FOR THE ENTIRE SIZE OF THE SHIP
                {
                    // CHECK IF AREA IS EMPTY
                    if (orientationNum == 0 && cells[row, col + j] != emptyCell) // horizontal && IF NEXT CELL (right) IS OCCUPIED
                    {
                        i--;
                        break;
                    }
                    else if (orientationNum == 1 && cells[row + j, col] != emptyCell) // vertical && IF NEXT CELL (down) IS OCCUPIED
                    {
                        i--;
                        break;
                    }
                    shipCells[j] = true;
                }
                if (shipCells.All(x => x == true)) // IF AREA IS CLEAR
                {
                    for (int k = 0; k < Ship.ships[i].Size; k++) // FILL AREA WITH SHIP
                    {
                        if (orientationNum == 0) // horizontal
                        {
                            cells[row, col + k] = Ship.ships[i].Symbol;
                        }
                        else // vertical
                        {
                            cells[row + k, col] = Ship.ships[i].Symbol;
                        }
                    }
                }
            }
        }
        public void PrintBoard()
        {
            string spcr;
            if (Fancy) { spcr = "| "; }
            else { spcr = ""; }
            char rowLetter = 'A';
            Console.WriteLine("    " + Name);
            Console.Write($"   {spcr}1 {spcr}2 {spcr}3 {spcr}4 {spcr}5 {spcr}6 {spcr}7 {spcr}8 {spcr}9 {spcr}10{spcr}");
            if (Fancy) { DrawGridSegment(); }
            else { Console.WriteLine(); }
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                Console.Write($" {(char)(rowLetter + i)} {spcr}");
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Console.Write($"{CellSpace(cells[i, j])}{spcr}");
                }
                if (Fancy) { DrawGridSegment(); }
                else { Console.WriteLine(); }
            }
        }
        public static void DrawGridSegment()
        {
            Console.WriteLine();
            for (int i = 0; i < 11; i++)
            {
                Console.Write("---┼"); // +
            }
            Console.WriteLine();
        }
        public static string CellSpace(string cellContents)
        {
            if (cellContents == "💥") { return cellContents; }
            return cellContents + " ";
        }
        public void ClearMisses()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    string emptyCell;
                    if (Fancy) { emptyCell = " "; }
                    else { emptyCell = "■"; }
                    if (cells[i, j] == "O") { cells[i, j] = emptyCell; }
                }
            }
        }
    }
}
