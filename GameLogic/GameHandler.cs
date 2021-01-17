using System;
using GameEntities;


namespace GameLogic
{
    public static class GameHandler
    {
        
        private static bool _currentMoveByPlayerOne = true;
        public static bool CurrentMoveByPlayerOne
        {
            get => _currentMoveByPlayerOne;
            set => _currentMoveByPlayerOne = value;
        }

        public static Player Player1 = null!;
        public static Player Player2 = null!;
        public static bool choosenShipIsAvailable;
        private static ShipLocator _currentLocator = null!;

        public static void ChosenShipToPlace()
        {
            bool isVertical;
        }

        //new 
        public static void CheckIsShipAvailable(Ship.ShipType type)
        {
            choosenShipIsAvailable = false;
            Player curPlayer = _currentMoveByPlayerOne ? Player1 : Player2;
            for (int i = 0; i < curPlayer.AvailableShips.Count; i++)
            {
                var sh = curPlayer.AvailableShips[i];
                //TODO переписать ! 
                if (sh.type == type)
                {
                    if (sh.numberOfShips > 0)
                    {
                        //ставим корабл
                        //CreateCurrentShipLocator(type);
                        choosenShipIsAvailable = true;
                        SetCurrentLocatorAviableShip(sh);
                        
                        Console.WriteLine($"Ship {type.ToString()} chosen for placement! available: {sh.numberOfShips.ToString()}");
                        break;
                    }
                    if (sh.numberOfShips == 0)
                    {
                        curPlayer.AvailableShips.Remove(sh);
                    }
                }
            }

        }

        private static void SetCurrentLocatorAviableShip(Player.AvailableShip availableShip)
        {
            _currentLocator.AvailableShip = availableShip;
            _currentLocator.ship.shipType = availableShip.type;
        }

        public static void SetShipLocatorPosition(int x, int y)
        {
            _currentLocator.X = x;
            _currentLocator.Y = y;
        }

        public static void CreateCurrentShipLocator(Ship.ShipType type = Ship.ShipType.Patrol)
        {
            Ship currentShip = new Ship(type, true, Ship.ShipState.Alive);
            _currentLocator = new ShipLocator(0, 0, currentShip);
        }

        //new 
        public static void PlaceShipForCurrentPlayer()
        {
            var curPlayer = _currentMoveByPlayerOne ? Player1 : Player2;
            curPlayer.OwnBoard.PlaceShip(_currentLocator.X, _currentLocator.Y, _currentLocator.ship.shipType,
                    _currentLocator.ship.isVertical, ref _currentLocator.AvailableShip.numberOfShips);
        }

        //new 
        public static int CurrentPlayerAvilableShipsCount()
        {
            int count = 0;
            Player curPlayer = _currentMoveByPlayerOne ? Player1 : Player2;

            for (int i = 0; i < curPlayer.AvailableShips.Count; i++)
            {
                count += curPlayer.AvailableShips[i].numberOfShips;
            }
            return count;
        }

        public static void SetCurrentPlacedShipVertical(bool isVertical)
        {
            _currentLocator.ship.isVertical = isVertical;
        }

        public static void PlayersMove()
        {
            int x;
            int y;

            Player attackingPlayer = _currentMoveByPlayerOne
                ? Player1
                : Player2;

            Player defendingPlayer = _currentMoveByPlayerOne
                ? Player2
                : Player1;

            Console.WriteLine("Enter x position of field to attack:");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out x) == false)
            {
                Console.WriteLine("Enter correct number");
            }

            Console.WriteLine("Enter y position of field to attack:");
            Console.Write(">");
            while (int.TryParse(Console.ReadLine(), out y) == false)
            {
                Console.WriteLine("Enter correct number");
            }

            switch (defendingPlayer.OwnBoard.GetCells()[x, y].GetCellState())
            {
                case Cell.CellState.Bombed:
                    Console.WriteLine("You have already attacked this ship. Try again");
                    break;
                case Cell.CellState.Missed:
                    Console.WriteLine("You have already attacked this field and it was empty. Try again");
                    break;
                case Cell.CellState.Empty:
                    defendingPlayer.OwnBoard.GetCells()[x, y].ChangeCellState(Cell.CellState.Missed);
                    attackingPlayer.AttackBoard.GetCells()[x, y].ChangeCellState(Cell.CellState.Missed);
                    Console.WriteLine("Unfortunately, this field is empty. It will be marked as missed now");
                    break;
                case Cell.CellState.Ship:
                    defendingPlayer.OwnBoard.GetCells()[x, y].ChangeCellState(Cell.CellState.Bombed);
                    attackingPlayer.OwnBoard.GetCells()[x, y].ChangeCellState(Cell.CellState.Bombed);
                    Console.WriteLine("Congats! You have hit the ship! Now you can make one more move!");
                    break;
            }
        }
        
        public static void ChangePlayerTurn()
        {
            if (_currentMoveByPlayerOne)
            {
                _currentMoveByPlayerOne = false;
            }
            else
            {
                _currentMoveByPlayerOne = false;
            }
        }
    }
}