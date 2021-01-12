using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using BattleShipConsoleUI;
using GameEntities;
using GameLogic;
using MenuSystem;

namespace BattleshipConsoleApp
{
    class ProgramMain
    {
        private static string jsonStateString;
        private static string savePath = "save.json";
        string userChoice = "";
        static Menu menuALevel = new Menu(MenuLevel.FirstWhetherReady);
        static Menu menuBLevel = new Menu(MenuLevel.SecondGameMode);
        static Menu menuShips = new Menu(MenuLevel.ShipsToPlace);
        static Menu menuPassOrExit = new Menu(MenuLevel.PassOrExit);
        private static GameState currentGameState;

        static void Main()
        {
            Console.WriteLine("<========== USELESS BATTLESHIP ==========>");
            Console.WriteLine("Welcome to the 'Pointless Battleship!'");
            Console.WriteLine("The most useless game you ever played!");
            Console.WriteLine("");
            Console.WriteLine("Are you ready? (y/n)");
            Console.WriteLine("");
            CreateMainMenuItems();
            menuALevel.RunMenu();
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
            FirstPlayerShipPlacement(width, height, menuShips);
            GameHandler.ChangePlayerTurn();
            SeconPlayerShipPlacement(width, height, menuShips);
            GameHandler.ChangePlayerTurn();
            SaveGameState();
            CreatePassMenuItems();
            GameFlow();
        }

        private static void GameFlow()
        {
            //GAME PROCCESS
            while (GameHandler.Player1.GetAmountOfBombedFields() <= 25 ||
                   GameHandler.Player2.GetAmountOfBombedFields() <= 25)
            {
                if (GameHandler.CurrentMoveByPlayerOne)
                {
                    Console.WriteLine("PLAYER'S 1 TURN: \n This is your board:");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player1.OwnBoard);
                    Console.WriteLine(
                        "This is board of your opponent (Player 2). \n It might seem empty now but you will fill it up.");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player1.AttackBoard);
                }
                else
                {
                    Console.WriteLine("PLAYER'S 2 TURN: \n This is your board:");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
                    Console.WriteLine(
                        "This is board of your opponent (Player 1). \n It might seem empty now but you will fill it up.");
                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.AttackBoard);
                }

                Console.WriteLine("Now make your next move:");
                GameHandler.PlayersMove();
                Console.WriteLine("Pass the deivce to the next player");
                GameHandler.ChangePlayerTurn();
                //menuPassOrExit.RunMenu();
            }
        }

        private static void CreatePassMenuItems()
        {
            menuPassOrExit.AddMenuItem(new MenuItem("p", "Pass", GameHandler.ChangePlayerTurn));
            menuPassOrExit.AddMenuItem(new MenuItem("s", "Save Game", SaveGameState));
            menuPassOrExit.AddMenuItem(new MenuItem("f", "Finish and exit", ClosingDown));
        }

        #region InitializeGameMethods

        private static void FirstPlayerShipPlacement(int width, int height, Menu menuShips)
        {
            //PLAYER ONE PLACES SHIPS
            Console.WriteLine("This is Player One board:");
            GameHandler.Player1.OwnBoard = new GameBoard(height, width, true);
            BattleShipConsoleUi.DrawBoard(GameHandler.Player1.OwnBoard);
            Console.WriteLine("Let's start placing ships one by one");
            Console.WriteLine("Choose a ship to place:");
            while (GameHandler.CurrentPlayerAvilableShipsCount() > 0)
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
                    Console.WriteLine("Ship is no available");
                }
            }

            //Create an attack board
            GameHandler.Player1.AttackBoard = new GameBoard(height, width, true);
        }

        private static void SeconPlayerShipPlacement(int width, int height, Menu menuShips)
        {
            //PLAYER TWO PLACES SHIPS
            Console.WriteLine("This is Player Two board:");
            GameHandler.Player2.OwnBoard = new GameBoard(height, width, true);
            BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
            Console.WriteLine("Let's start placing ships one by one");
            Console.WriteLine("Choose a ship to place:");
            while (GameHandler.CurrentPlayerAvilableShipsCount() > 0)
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

                    BattleShipConsoleUi.DrawBoard(GameHandler.Player2.OwnBoard);
                }
                else
                {
                    Console.WriteLine("Ship is no available");
                }
            }

            //Create an attack board
            GameHandler.Player2.AttackBoard = new GameBoard(height, width, true);
        }

        private static void PlayerChooseOrientation()
        {
            Console.WriteLine(" v for vertical orientation \n h for horizontal orientation");
            Console.Write(">");
            while (true)
            {
                string answer = Console.ReadLine();
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
                    Console.WriteLine("Enter correct value");
                }
            }
        }

        private static void SetBoardSize(out int width, out int height)
        {
            Console.WriteLine("Enter width of the board");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out width) == false)
            {
                Console.WriteLine("Enter correct number");
            }

            Console.WriteLine("Enter height of the board");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out height) == false)
            {
                Console.WriteLine("Enter correct number");
            }
        }

        private static void CreateShipMenuItems(Menu menuShips)
        {
            menuShips.AddMenuItem(
                new MenuItem("ca", "Carrier - ◘◘◘◘◘", () => SetPlacingShipType(Ship.ShipType.Carrier)));
            menuShips.AddMenuItem(new MenuItem("ba", "Battleship - ◘◘◘◘",
                () => SetPlacingShipType(Ship.ShipType.Battleship)));
            menuShips.AddMenuItem(new MenuItem("su", "Submarine - ◘◘◘",
                () => SetPlacingShipType(Ship.ShipType.Submarine)));
            menuShips.AddMenuItem(new MenuItem("cr", "Cruiser - ◘◘", () => SetPlacingShipType(Ship.ShipType.Cruiser)));
            menuShips.AddMenuItem(new MenuItem("pa", "Patrol - ◘", () => SetPlacingShipType(Ship.ShipType.Patrol)));
            menuShips.AddMenuItem(new MenuItem("s", "Save Game State", SaveGameState));
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
            Console.WriteLine("Enter x position of the chosen ship");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out x) == false)
            {
                Console.WriteLine("Enter correct number");
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
                Console.WriteLine("Enter correct number");
            }

            return y;
        }

        static void ClosingDown()
        {
            Console.WriteLine("Bye");
        }

        static void LoadGameState()
        {
            jsonStateString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, savePath));
            currentGameState = JsonSerializer.Deserialize<GameState>(jsonStateString);
            GameHandler.Player1 = currentGameState.PlayerOne;
            GameHandler.Player2 = currentGameState.PlayerTwo;
            GameHandler.CurrentMoveByPlayerOne = currentGameState.CurrentMoveByPlayerOne;
            //currentGameState.PlayerOne.FillBoardsAfterLoad(currentGameState.PlayerOne.ownSavedBoard, currentGameState.PlayerOne.attackSavedBoard);
            //currentGameState.PlayerTwo.FillBoardsAfterLoad(currentGameState.PlayerTwo.ownSavedBoard, currentGameState.PlayerTwo.attackSavedBoard);
            GameFlow();
        }

        static void SaveGameState()
        {
            currentGameState = new GameState();
            currentGameState.CurrentMoveByPlayerOne = GameHandler.CurrentMoveByPlayerOne;
            currentGameState.PlayerOne = GameHandler.Player1;
            currentGameState.PlayerTwo = GameHandler.Player2;
            currentGameState.PlayerOne.FillDicts();
            currentGameState.PlayerTwo.FillDicts();
            jsonStateString = JsonSerializer.Serialize(currentGameState, typeof(GameState));
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory,savePath), jsonStateString);
            Console.WriteLine(Path.Combine(Environment.CurrentDirectory,savePath));
        }
    }
}