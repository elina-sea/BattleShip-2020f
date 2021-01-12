using System;
using System.Collections.Generic;
using System.IO;

namespace GameEntities
{
    [System.Serializable]
    public class Player
    {


        
        public Player()
        {
        }

        public void FillBoardsAfterLoad(List<Cell> loadOwnBoard, List<Cell> loadAttackBoard)
        {
            foreach (var cell in loadOwnBoard)
            {
                OwnBoard._board[cell._xPosition, cell._yPosition] = cell;
            }
            foreach (var cell in loadAttackBoard)
            {
                AttackBoard._board[cell._xPosition, cell._yPosition] = cell;
            }
        }
        public GameBoard OwnBoard { get; set; }
        public GameBoard AttackBoard { get; set; }
        public PlayerNumber PlayerNum;
        public List<AvailableShip> AvailableShips;
        public List<Cell> ownSavedBoard { get; set; }
        public List<Cell> attackSavedBoard { get; set; }

        public void FillDicts()
        {
            ownSavedBoard = new List<Cell>();
            
            attackSavedBoard = new List<Cell>();
            for (int x = 0; x < OwnBoard._board.GetLength(0);x++)
            {
                for (int y = 0; y < OwnBoard._board.GetLength(1);y++)
                {
                    ownSavedBoard.Add(OwnBoard._board[x,y]);
                }
            }
            for (int x = 0; x < AttackBoard._board.GetLength(0);x++)
            {
                for (int y = 0; y < AttackBoard._board.GetLength(1);y++)
                {
                    ownSavedBoard.Add(AttackBoard._board[x,y]);
                }
            }
        }

        public enum PlayerNumber
        {
            PalyerOne, PlayerTwo
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