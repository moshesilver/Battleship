using System.Text;
using CommonConsoleAppMethods;

namespace Battleship
{
    class Game
    {
        public static Board playerBoard;
        public static Board computerBoard;
        public static int turnCount;
        public static bool gameOver;
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            PlayAgain();
        }
        public static void InitializeGlobals()
        {
            playerBoard = new($"{Methods.GetName("Player name")}'s Board", Methods.YesNoInput("Fancy board"));
            computerBoard = new("Computer Board", playerBoard.Fancy);
            turnCount = 0;
            gameOver = false;
        }
        public static void CreateShips()
        {
            new Ship("Aircraft Carrier", 5);
            new Ship("Battleship", 4);
            new Ship("Cruiser", 3);
            new Ship("Submarine", 3);
            new Ship("Destroyer", 2);
        }
        public static void GetPlayerGuess(out int ltr, out int num)
        {
            string? letter;
            ltr = -1;
            do
            {
                Console.Write("Choose a row (A - J): ");
                letter = Console.ReadLine();
                Methods.ClearConsoleLine();
                if (!string.IsNullOrEmpty(letter))
                {
                    letter = letter.Trim();
                    if (letter.Length == 1)
                    {
                        ltr = (Convert.ToChar(letter.ToUpper()) - 65);
                    }
                }
            } while (ltr < 0 || ltr > 9);
            num = Methods.GetNumber(1, 10, "Choose a column (1 - 10)") - 1;
        }
        public static string ExecutePlayerGuess(int ltr, int num)
        {
            return computerBoard.cells[ltr, num] switch
            {
                " " => SetCell(ltr, num, "O"),
                "■" => SetCell(ltr, num, "O"),
                "O" => AlreadyRevealed("O"),
                "💥" => AlreadyRevealed("💥"),
                _ => SetCell(ltr, num, "💥")
            };
        }
        public static string SetCell(int ltr, int num, string contents)
        {
            playerBoard.cells[ltr, num] = contents;
            foreach (Ship ship in Ship.ships)
            {
                if (ship.Symbol == computerBoard.cells[ltr, num])
                {
                    ship.Hits++;
                }
            }
            computerBoard.cells[ltr, num] = contents;
            foreach (Ship ship in Ship.ships)
            {
                if (ship.Sunk && !ship.Announced)
                {
                    Console.Clear();
                    playerBoard.PrintBoard();
                    Console.WriteLine($"Congratulations: You sunk my {ship.Name}!");
                    ship.Announced = true;
                    Thread.Sleep(2000);
                }
            }
            return contents;
        }
        public static string AlreadyRevealed(string cellContents)
        {
            Console.WriteLine($"Position status already revealed");
            Thread.Sleep(2000);
            if (cellContents == "💥") { return "💥"; }
            return "O";
        }
        public static void Turn()
        {
            GetPlayerGuess(out int ltr, out int num);
            ExecutePlayerGuess(ltr, num);
            Console.Clear();
            playerBoard.PrintBoard();
            if (Ship.ships.All(ship => ship.Sunk)) { Win(); }
            else if (turnCount >= 42) { Lose(); }
            turnCount++;
        }
        public static void Win()
        {
            computerBoard.ClearMisses();
            gameOver = true;
            BurningShipAnimation();
            Thread.Sleep(3000);
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                Thread.Sleep(500);
                Console.WriteLine("😎Congratulations: YOU WIN!😎");
                computerBoard.PrintBoard();
                Thread.Sleep(500);
            }
            GetReplay();
        }
        public static void Lose()
        {
            computerBoard.ClearMisses();
            gameOver = true;
            Console.Clear();
            Console.WriteLine("😢Sorry: You Lose😢\nBetter luck next time!");
            computerBoard.PrintBoard();
            GetReplay();
        }
        public static void GetReplay()
        {
            if (Methods.YesNoInput("Do you want to play again"))
            {
                PlayAgain();
            }
            else { Console.WriteLine("See you next time!"); }
        }
        public static void PlayAgain()
        {
            Console.Clear();
            InitializeGlobals();
            CreateShips();
            computerBoard.PlaceShips();
            playerBoard.PrintBoard();
            while (!gameOver) { Turn(); }
        }
        public static void BurningShipAnimation()
        {
            Console.Clear();
            Console.WriteLine(
"                                       🔥🔥🔥  💣\n" +
"                                        🔥🔥 💣💣\n" +
"                                   🔥🔥  🔥🔥🔥💣\n" +
"                                🔥  🔥 🔥🔥🔥💣💣\n" +
"                              🔥🔥🔥 🔥🔥🔥🔥💣💣\n" +
"                                 🔥🔥🔥🔥🔥💣💣💣\n" +
"                                   💣🔥🔥💣💣💣💣\n" +
"                                 💣💣💣💣💣💣💣💣\n" +
"                               💣💣💣💣💣💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"                                         💣💣💣💣\n" +
"    🔥               🔥                  💣💣💣💣\n" +
"   🔥🔥  🔥        🔥🔥                  💣💣💣💣   🔥  🔥   🔥  🔥\n" +
"    🔥🔥🔥      🔥🔥🔥  🔥🔥             💣💣💣💣    🔥🔥   🔥🔥🔥🔥\n" +
" 🔥  🔥🔥🔥🔥🔥🔥🔥  🔥🔥🔥		 💣💣💣💣   🔥🔥      🔥🔥🔥\n" +
"🔥🔥  🔥🔥🔥🔥🔥🔥  🔥🔥                 💣💣💣💣    🔥🔥🔥    🔥🔥\n" +
"   🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥	                 💣💣💣💣     🔥🔥🔥🔥🔥🔥\n" +
"    🔥🔥🔥🔥🔥🔥🔥🔥	                 💣💣💣💣    🔥🔥🔥🔥🔥🔥🔥\n" +
"💣🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣🔥🔥🔥🔥🔥🔥💣💣💣💣\n" +
" 💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"  💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"   💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"    💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"     💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"      💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"       💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣\n" +
"        💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣💣");
        }
    }
}
