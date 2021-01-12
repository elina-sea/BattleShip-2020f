using System;
using System.Collections.Generic;

namespace GameEntities
{
    [System.Serializable]
    public class GameBoard
    {
        public GameBoard(int height, int width)
        {
            _height = height;
            _width = width;
        }
        
        public GameBoard()
        {
        }

        public int _height { get; set; }
        public int _width { get; set; }
        public Cell[,] _board;

        public GameBoard(int height, int width, bool initialize)
        {
            this._height = height;
            this._width = width;
            _board = new Cell[this._height, this._width]; //create board as an array of cells
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _board[i, j] = new Cell(Cell.CellState.Empty);
                    _board[i, j]._xPosition = i;
                    _board[i, j]._yPosition = j;
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
                    var lengthY = _board.GetLength(1);
                    if (lengthY <= startPosition + shipSize - 1 || _board[x, i].IsAvailable() == false) //if unavailable
                    {
                        placeAvailable = false;
                    }
                }
                else //x
                {
                    var lengthX = _board.GetLength(0);
                    if (lengthX <= startPosition + shipSize - 1 || _board[i, y].IsAvailable() == false) //if unavailable
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
                    _board[x, i].SetState(Cell.CellState.Ship);
                }

                Console.WriteLine($"Ship placed on position x = {x + 1}, y = {y + 1}");
            }

            if (placeAvailable && !isVertical)
            {
                availableCount--;
                for (int i = startPosition; i < (startPosition + shipSize); i++)
                {
                    _board[i, y].SetState(Cell.CellState.Ship);
                }

                Console.WriteLine($"Ship placed on position x = {x + 1}, y = {y + 1}");
            }
        }

        public Cell[,] GetCells()
        {
            return _board;
        }
    }
}