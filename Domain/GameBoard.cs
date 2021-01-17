using System;
using System.Collections.Generic;

namespace GameEntities
{
    [System.Serializable]
    public class GameBoard
    {
        //PK
        public int GameBoardId { get; set; }
        //FK
        public int PlayerId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        //public ICollection<Cell> Board { get; set; } = null!;
        public Cell[,] Board { get; set; } = null!;

        public GameBoard()
        {
        }
        
        public GameBoard(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public GameBoard(int height, int width, bool initialize)
        {
            this.Height = height;
            this.Width = width;
            Board = new Cell[this.Height, this.Width]; //create board as an array of cells
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Board[i, j] = new Cell(Cell.CellState.Empty);
                    Board[i, j].XPosition = i;
                    Board[i, j].YPosition = j;
                }
            }
        }

        public void PlaceShip(int x, int y, Ship.ShipType shipType, bool isVertical, ref int availableCount)
        {
            int shipSize = Ship.GetShipSizeByShipType(shipType);
            int startPosition = (isVertical) ? y : x;
            bool placeAvailable = true;

            for (int i = startPosition; i < (startPosition + shipSize); i++)
            {
                if (isVertical) //y
                {
                    var lengthY = Board.GetLength(1);
                    if (lengthY <= startPosition + shipSize - 1 || Board[x, i].IsAvailable() == false) //if unavailable
                    {
                        placeAvailable = false;
                    }
                }
                else //x
                {
                    var lengthX = Board.GetLength(0);
                    if (lengthX <= startPosition + shipSize - 1 || Board[i, y].IsAvailable() == false) //if unavailable
                    {
                        placeAvailable = false;
                    }
                }
            }

            if (placeAvailable == false)
            {
                System.Console.WriteLine("Ship placement is not available at choosen point ");
                return;
            }

            if (placeAvailable && isVertical)
            {
                availableCount--;
                for (int i = startPosition; i < (startPosition + shipSize); i++)
                {
                    Board[x, i].SetState(Cell.CellState.Ship);
                }

                Console.WriteLine($"Ship placed on position x = {x + 1}, y = {y + 1}");
            }

            if (placeAvailable && !isVertical)
            {
                availableCount--;
                for (int i = startPosition; i < (startPosition + shipSize); i++)
                {
                    Board[i, y].SetState(Cell.CellState.Ship);
                }

                Console.WriteLine($"Ship placed on position x = {x + 1}, y = {y + 1}");
            }
        }

        public Cell[,] GetCells()
        {
            return Board;
        }
    }
}