using System;
using System.Collections.Generic;

namespace GameEntities
{
    [Serializable]
    public class Player
    {
        //PK
        //public int PlayerId { get; set; }
        //public int OwnBoardId { get; set; }
        public GameBoard OwnBoard { get; set; } = null!;
        //public int AttackBoardId { get; set; }
        public GameBoard AttackBoard { get; set; } = null!;
        public PlayerNumber PlayerNum;
        public List<AvailableShip> AvailableShips = null!;
        public Cell[] OwnSavedBoard { get; set; } = null!;
        public Cell[] AttackSavedBoard { get; set; } = null!;

        public Player()
        {
        }

        public void FillBoardsAfterLoad(Cell[] loadOwnBoard, Cell[] loadAttackBoard)
        {
            OwnBoard.Board = new Cell[OwnBoard.Height, OwnBoard.Width];
            AttackBoard.Board = new Cell[AttackBoard.Height, AttackBoard.Width];
            foreach (var cell in loadOwnBoard)
            {
                OwnBoard.Board[cell.XPosition, cell.YPosition] = cell;
            }

            foreach (var cell in loadAttackBoard)
            {
                AttackBoard.Board[cell.XPosition, cell.YPosition] = cell;
            }

            Console.WriteLine(OwnBoard.Board.Length);
        }

        public void FillArrays()
        {
            OwnSavedBoard = new Cell[OwnBoard.GetCells().Length];
            AttackSavedBoard = new Cell[AttackBoard.GetCells().Length];
            int iterator = 0;
            for (int x = 0; x < OwnBoard.Board.GetLength(0); x++)
            {
                for (int y = 0; y < OwnBoard.Board.GetLength(1); y++)
                {
                    OwnSavedBoard[iterator] = OwnBoard.Board[x, y];
                    iterator++;
                }
            }

            iterator = 0;
            for (int x = 0; x < AttackBoard.Board.GetLength(0); x++)
            {
                for (int y = 0; y < AttackBoard.Board.GetLength(1); y++)
                {
                    AttackSavedBoard[iterator] = AttackBoard.Board[x, y];
                    iterator++;
                }
            }
        }


        public enum PlayerNumber
        {
            PalyerOne,
            PlayerTwo
        }

        public class AvailableShip
        {
            public Ship.ShipType type;
            public int numberOfShips;

            public AvailableShip(Ship.ShipType type, int numberOfShips)
            {
                this.type = type;
                this.numberOfShips = numberOfShips;
            }
        }


        public Player(PlayerNumber num, int height, int width)
        {
            PlayerNum = num;
            OwnBoard = new GameBoard(height, width, true);
            AttackBoard = new GameBoard(height, width, true);
            AvailableShips = new List<AvailableShip>();
            AvailableShips.Add(new AvailableShip(Ship.ShipType.Carrier, 1));
            AvailableShips.Add(new AvailableShip(Ship.ShipType.Battleship, 1));
            AvailableShips.Add(new AvailableShip(Ship.ShipType.Submarine, 2));
            AvailableShips.Add(new AvailableShip(Ship.ShipType.Cruiser, 3));
            AvailableShips.Add(new AvailableShip(Ship.ShipType.Patrol, 4));
        }

        public int GetAmountOfBombedFields()
        {
            int count = 0;

            foreach (var cell in this.OwnBoard.GetCells())
            {
                if (cell.GetCellState() == Cell.CellState.Bombed)
                {
                    count++;
                }
            }

            return count;
        }
    }
}