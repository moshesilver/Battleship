using System.Text;

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
            playerBoard = new($"{GetPlayerName()}'s Board", IsFancyBoard());
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
        public static string GetPlayerName()
        {
            string? playerName;
            do
            {
                Console.Write("Player name: ");
                playerName = Console.ReadLine();
                ClearCurrentConsoleLine();
            } while (string.IsNullOrEmpty(playerName));
            string[] firstLetters = playerName.Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            playerName = "";
            for (int i = 0; i < firstLetters.Length; i++)
            {
                firstLetters[i] = firstLetters[i].Replace(firstLetters[i][0], char.ToUpper(firstLetters[i][0]));
                playerName += firstLetters[i];
                if (i != firstLetters.Length - 1)
                {
                    playerName += " ";
                }
            }
            return playerName;
        }
        public static bool IsFancyBoard()
        {
            string? isFancyBoard;
            do
            {
                Console.Write("Fancy board? (Y/N): ");
                isFancyBoard = Console.ReadLine();
                ClearCurrentConsoleLine();
            } while (string.IsNullOrEmpty(isFancyBoard));
            isFancyBoard = isFancyBoard.Trim().ToUpper();
            if (isFancyBoard == "Y" || isFancyBoard == "YES")
            {
                return true;
            }
            return false;
        }
        public static void GetPlayerGuess(out int ltr, out int num)
        {
            string? letter;
            string? numLetter;
            ltr = -1;
            num = -1;
            Console.WriteLine();
            do
            {
                ClearCurrentConsoleLine();
                Console.Write("Choose a row (A - J): ");
                letter = Console.ReadLine();
                if (!string.IsNullOrEmpty(letter))
                {
                    letter = letter.Trim();
                    if (letter.Length == 1)
                    {
                        ltr = (Convert.ToChar(letter.ToUpper()) - 65);
                    }
                }
            } while (ltr < 0 || ltr > 9);
            Console.WriteLine();
            do
            {
                ClearCurrentConsoleLine();
                Console.Write("Choose a column (1 - 10): ");
                numLetter = Console.ReadLine();
                int.TryParse(numLetter, out num);
            } while (num < 1 || num > 10);
            num--;
        }
        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
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
            string? playAgain;
            do
            {
                Console.WriteLine("Do you want to play again?: ");
                playAgain = Console.ReadLine();
                ClearCurrentConsoleLine();
            } while (string.IsNullOrEmpty(playAgain));
            playAgain = playAgain.Trim().ToUpper();
            if (playAgain == "Y" || playAgain == "YES")
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
