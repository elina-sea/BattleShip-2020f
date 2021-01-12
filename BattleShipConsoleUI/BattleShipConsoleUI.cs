using System;
using GameEntities;

namespace BattleShipConsoleUI
{
    public static class BattleShipConsoleUi
    {
        public static void DrawBoard(GameBoard gameBoard)
        {
            Cell[,]board = gameBoard.GetCells();
            var width = board.GetUpperBound(0) + 1; // 0 - of first array (of x)
            var height = board.GetUpperBound(1) + 1; // 1 - of second array (of y)

            for (int columns = 0; columns < width; columns++)
            {
                //TODO add ident. numbers of fields
                Console.Write("+---+");
            }
            Console.WriteLine();

            for (int rows = 0; rows < height; rows++)
            {
                for (int columns = 0; columns < width; columns++)
                {
                    Console.Write($"| {PrintCellState(board[columns,rows].GetCellState())} |"); 
                }
                Console.WriteLine();
                for (int columns = 0; columns < width; columns++)
                {
                    Console.Write($"+---+");
                }
                Console.WriteLine();
            }
        }

        public static string PrintCellState(Cell.CellState state)
        {
            switch (state)
            {
                case Cell.CellState.Bombed:
                    return "x";
                case Cell.CellState.Missed:
                    return "o";
                case Cell.CellState.Empty:
                    return " ";
                case Cell.CellState.Ship:
                    return "◘";
            }
            
            return "";
        }
    }
}