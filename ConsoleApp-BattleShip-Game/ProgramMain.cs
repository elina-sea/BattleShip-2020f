using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Channels;
using BattleShipConsoleUI;
using Domain;
using GameEntities;
using GameLogic;
using MenuSystem;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace BattleshipConsoleApp
{
    class ProgramMain
    {
        private static string? _jsonStateString;
        private static string savePath = "save.json";
        public string _userChoice = "";
        static Menu menuALevel = new Menu(MenuLevel.FirstWhetherReady);
        static Menu menuBLevel = new Menu(MenuLevel.SecondGameMode);
        static Menu _menuShips = new Menu(MenuLevel.ShipsToPlace);
        static Menu menuPassOrExit = new Menu(MenuLevel.PassOrExit);
        private static GameState _currentGameState = null!;
        private static bool _shipMenuExit;

        static void Main()
        {
            //TODO rename
            Console.WriteLine("<========== THE BEST BATTLESHIP EVER CREATED (...by me) ==========>");
            Console.WriteLine("Welcome to THE GREATEST Battleship!");
            Console.WriteLine(
                "Rules are simple: \n - Place ships (then game will be saved)\n - Shoot each other's ships\n - Win or be the loser!");
            Console.WriteLine("");
            Console.WriteLine("Are you ready? (y/n)");
            Console.WriteLine("");
            CreateMainMenuItems();
            Console.WriteLine("========================");
            menuALevel.RunMenu();
            Console.WriteLine("========================");
        }

        private static void CreateMainMenuItems()
        {
            menuBLevel.AddMenuItem(new MenuItem("1", "Start Game", StartGame));
            menuBLevel.AddMenuItem(new MenuItem("2", "Continue previous game", LoadGameState));
            menuALevel.AddMenuItem(new MenuItem("y", "Yes", menuBLevel.RunMenu));
            menuALevel.AddMenuItem(new MenuItem("n", "No", ClosingDown));
        }

        static void StartGame()
        {
            int width, height;
            var menuShips = new Menu(MenuLevel.ShipsToPlace);
            List<ShipLocator> shipsOnBoard = new List<ShipLocator>();

            CreateShipMenuItems(menuShips);
            SetBoardSize(out width, out height);

            //PLAYER INITIALIZATION
            GameHandler.Player1 = new Player(Player.PlayerNumber.PalyerOne, height, width);
            GameHandler.Player2 = new Player(Player.PlayerNumber.PlayerTwo, height, width);
            //BOARDS' CREATION AND SAVING
            _shipMenuExit = false;
            FirstPlayerShipPlacement(width, height, menuShips);
            GameHandler.ChangePlayerTurn();
            SeconPlayerShipPlacement(width, height, menuShips);
            GameHandler.ChangePlayerTurn();
            SaveGameState();
            GameFlow();
        }

        private static void GameFlow()
        {
            Console.WriteLine("===========================");
            Console.WriteLine("");
            Console.WriteLine(" NOW LET'S START THE GAME");
            Console.WriteLine("");
            Console.WriteLine("===========================");
            CreatePassMenuItems();
            //GAME PROCCESS
            while (GameHandler.Player1.GetAmountOfBombedFields() <= 25 ||
                   GameHandler.Player2.GetAmountOfBombedFields() <= 25)
            {
                if (GameHandler.CurrentMoveByPlayerOne)
                {
                    Console.WriteLine("");
                    Console.WriteLine("PLAYER'S 1 TURN: \n This is your board:");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player1.OwnBoard);
                    Console.WriteLine(
                        "This is board of your opponent (Player 2). \n It might seem empty now but you will fill it up.");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player1.AttackBoard);
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("PLAYER'S 2 TURN: \n This is your board:");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
                    Console.WriteLine(
                        "This is board of your opponent (Player 1). \n It might seem empty now but you will fill it up.");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.AttackBoard);
                }

                Console.WriteLine("Now make your next move:");
                GameHandler.PlayersMove();
                SaveGameState();
                Console.WriteLine("=====================================");
                Console.WriteLine(" Pass the device to the next player?");
                Console.WriteLine(" Remember to hit `s` if you want to save and continue your game later!");
                menuPassOrExit.RunMenu();
                Console.WriteLine("=====================================");
            }
        }

        private static void CreatePassMenuItems()
        {
            menuPassOrExit.AddMenuItem(new MenuItem("p", "Pass", () => Console.WriteLine("Switching user...")));
            menuPassOrExit.AddMenuItem(new MenuItem("s", "Save Game", SaveGameState));
            menuPassOrExit.AddMenuItem(new MenuItem("f", "Finish and exit (save before this!)",
                ClosingDown));
        }

        #region InitializeGameMethods

        private static void FirstPlayerShipPlacement(int width, int height, Menu menuShips)
        {
            //PLAYER ONE PLACES SHIPS
            Console.WriteLine("===========================");
            Console.WriteLine(" This is Player One board:");
            GameHandler.Player1.OwnBoard = new GameBoard(height, width, true);
            BattleShipConsoleUi.DrawBoard(GameHandler.Player1.OwnBoard);
            Console.WriteLine("Let's start placing ships one by one");
            Console.WriteLine("Choose a ship to place:");
            Console.WriteLine("===========================");
            while (GameHandler.CurrentPlayerAvilableShipsCount() > 0 && _shipMenuExit == false)
            {
                Console.WriteLine($"\nAviable Ships for p1: {GameHandler.CurrentPlayerAvilableShipsCount()}");
                //new 
                GameHandler.CreateCurrentShipLocator();
                menuShips.RunMenu(); // if ship is available then GameHandler.choosenShipIsAvailable = true other way false
                if (GameHandler.choosenShipIsAvailable)
                {
                    PlayerChooseOrientation();
                    int x = UserChoiceXPosition();
                    int y = UserChoiceYPosition();
                    GameHandler.SetShipLocatorPosition(x - 1, y - 1);
                    GameHandler.PlaceShipForCurrentPlayer();

                    BattleShipConsoleUi.DrawBoard(GameHandler.Player1.OwnBoard);
                }
                else
                {
                    Console.WriteLine("=======================");
                    Console.WriteLine(" Ship is not available,\nChoose another one");
                    Console.WriteLine("=======================");
                }
            }

            //Create an attack board
            GameHandler.Player1.AttackBoard = new GameBoard(height, width, true);
        }

        private static void SeconPlayerShipPlacement(int width, int height, Menu menuShips)
        {
            //PLAYER TWO PLACES SHIPS
            Console.WriteLine("===========================");
            Console.WriteLine(" This is Player Two board:");
            GameHandler.Player2.OwnBoard = new GameBoard(height, width, true);
            BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
            Console.WriteLine("Let's start placing ships one by one");
            Console.WriteLine("Choose a ship to place:");
            Console.WriteLine("===========================");
            while (GameHandler.CurrentPlayerAvilableShipsCount() > 0 && _shipMenuExit == false)
            {
                Console.WriteLine($"\nAviable Ships for p2: {GameHandler.CurrentPlayerAvilableShipsCount()}");
                //new 
                GameHandler.CreateCurrentShipLocator();
                menuShips.RunMenu(); // if ship is available then GameHandler.choosenShipIsAvailable = true other way false
                if (GameHandler.choosenShipIsAvailable)
                {
                    PlayerChooseOrientation();
                    int x = UserChoiceXPosition();
                    int y = UserChoiceYPosition();
                    GameHandler.SetShipLocatorPosition(x - 1, y - 1);
                    GameHandler.PlaceShipForCurrentPlayer();

                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
                }
                else
                {
                    Console.WriteLine("=======================");
                    Console.WriteLine(" Ship is not available,\nChoose another one");
                    Console.WriteLine("=======================");
                }
            }

            //Create an attack board
            GameHandler.Player2.AttackBoard = new GameBoard(height, width, true);
        }

        private static void PlayerChooseOrientation()
        {
            Console.WriteLine("============================");
            Console.WriteLine(" v for vertical orientation \n h for horizontal orientation");
            Console.Write(">");
            while (true)
            {
                string answer = Console.ReadLine()!;
                if (answer == "v")
                {
                    GameHandler.SetCurrentPlacedShipVertical(true);
                    break;
                }
                else if (answer == "h")
                {
                    GameHandler.SetCurrentPlacedShipVertical(false);
                    break;
                }
                else
                {
                    Console.WriteLine("=====================");
                    Console.WriteLine(" Enter correct value");
                    Console.WriteLine("=====================");
                }
            }
        }

        private static void SetBoardSize(out int width, out int height)
        {
            Console.WriteLine("==========================");
            Console.WriteLine(" Enter width of the board");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out width) == true)
            {
                if (width >= 7 && width <= 30)
                    break;
                Console.WriteLine("===================================================");
                Console.WriteLine(" You entered wrong type or number is too small/big \n (minimum board size is 6x6, maximum - 30x30)");
                Console.WriteLine("===================================================");
            }
            Console.WriteLine("==========================");
            Console.WriteLine("Enter height of the board");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out height) == true)
            {
                if (width >= 7 && height <= 30)
                    break;
                Console.WriteLine("===================================================");
                Console.WriteLine(" You entered wrong type or number is too small/big \n (minimum board size is 6x6, maximum - 30x30)");
                Console.WriteLine("===================================================");
            }
        }

        private static void CreateShipMenuItems(Menu menuShips)
        {
            menuShips.AddMenuItem(
                new MenuItem("ca", "Carrier - ◘◘◘◘◘", 
                    () => SetPlacingShipType(Ship.ShipType.Carrier)));
            menuShips.AddMenuItem(new MenuItem("ba", "Battleship - ◘◘◘◘",
                () => SetPlacingShipType(Ship.ShipType.Battleship)));
            menuShips.AddMenuItem(new MenuItem("su", "Submarine - ◘◘◘",
                () => SetPlacingShipType(Ship.ShipType.Submarine)));
            menuShips.AddMenuItem(new MenuItem("cr", "Cruiser - ◘◘",
                () => SetPlacingShipType(Ship.ShipType.Cruiser)));
            menuShips.AddMenuItem(new MenuItem("pa", "Patrol - ◘",
                () => SetPlacingShipType(Ship.ShipType.Patrol)));
            menuShips.AddMenuItem(new MenuItem("f", "Finish and exit (will not be saved)",
                ClosingDown));
            menuShips.AddMenuItem(new MenuItem("r", "Return to the main menu",
                ExitToMainMenu));
            // this is for debugging only
            menuShips.AddMenuItem(new MenuItem("s", "Save Game", SaveGameState));
        }

        private static void ExitToMainMenu()
        {
            _shipMenuExit = true;
            Console.WriteLine("========================");
            menuBLevel.RunMenu();
            Console.WriteLine("========================");
        }

        #endregion

        static void DefaultMenuAction()
        {
            Console.WriteLine("Default Menu Action");
        }

        //new (renamed)
        static void SetPlacingShipType(Ship.ShipType type)
        {
            GameHandler.CheckIsShipAvailable(type);
        }

        static int UserChoiceXPosition()
        {
            int x;
            Console.WriteLine("=====================================");
            Console.WriteLine(" Enter x position of the chosen ship");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out x) == false)
            {
                Console.WriteLine("======================");
                Console.WriteLine(" Enter correct number");
                Console.WriteLine("======================");
            }

            return x;
        }

        static int UserChoiceYPosition()
        {
            int y;
            Console.WriteLine("Enter y position of the chosen ship");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out y) == false)
            {
                Console.WriteLine("======================");
                Console.WriteLine(" Enter correct number");
                Console.WriteLine("======================");
            }

            return y;
        }

        static void ClosingDown()
        {
            Console.WriteLine("================================");
            Console.WriteLine(" Oke, see you next time! In the \n<=== BEST BATTLESHIP EVER ===> \n Bye bye, captain!");
            Environment.Exit(0);
        }

        static void SaveDeserializedString(string s)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "loaded.json"), s);
        }

        static void LoadGameState()
        {
            //THIS IS DB PART
            using var db = new AppDbContext();
            List<SavedGameData> data = db.SavedGameDataEntries.ToList();
            _jsonStateString = data.Last().JsonData;

            // THIS IS JSON PART
            //_jsonStateString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, savePath));
            _currentGameState = JsonSerializer.Deserialize<GameState>(_jsonStateString);
            _currentGameState!.PlayerOne.FillBoardsAfterLoad(_currentGameState.PlayerOne.OwnSavedBoard,
                _currentGameState.PlayerOne.AttackSavedBoard);
            _currentGameState.PlayerTwo.FillBoardsAfterLoad(_currentGameState.PlayerTwo.OwnSavedBoard,
                _currentGameState.PlayerTwo.AttackSavedBoard);
            SaveDeserializedString(_jsonStateString);
            GameHandler.Player1 = _currentGameState.PlayerOne;
            GameHandler.Player2 = _currentGameState.PlayerTwo;
            GameHandler.CurrentMoveByPlayerOne = _currentGameState.CurrentMoveByPlayerOne;
            GameFlow();
        }

        static void SaveGameState()
        {
            //SERIALIZATION
            _currentGameState = new GameState();
            _currentGameState.CurrentMoveByPlayerOne = GameHandler.CurrentMoveByPlayerOne;
            _currentGameState.PlayerOne = GameHandler.Player1;
            _currentGameState.PlayerTwo = GameHandler.Player2;
            _currentGameState.PlayerOne.FillArrays();
            _currentGameState.PlayerTwo.FillArrays();
            _jsonStateString = JsonSerializer.Serialize(_currentGameState, typeof(GameState));
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, savePath), _jsonStateString);
            //Console.WriteLine(Path.Combine(Environment.CurrentDirectory, savePath));

            //DATABASE
            using var db = new AppDbContext();
            db.Database.Migrate();
            SavedGameData data = new SavedGameData()
            {
                JsonData = _jsonStateString,
                TimeStamp = DateTime.Now.ToLongDateString()
            };
            db.Add(data);
            //Console.WriteLine(data.JsonData);
            //Console.WriteLine(data.TimeStamp);
            db.SaveChanges();
        }
    }
}