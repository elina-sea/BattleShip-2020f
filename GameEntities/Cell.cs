using System;

namespace GameEntities
{
    [System.Serializable]
    public class Cell
    {
        public int _xPosition { get; set; }
        public int _yPosition { get; set; }

        public Cell(CellState state)
        {
            _state = state;
        }
        
        public Cell()
        {
        }

        [Serializable]
        public enum CellState //перечисление
        {
            Empty,
            Missed,
            Bombed,
            Ship
        }

        public CellState _state { get; set; } //implements enumered entities => state.empty

        public bool IsAvailable()
        {
            return _state == CellState.Empty; //return true or false corresponding to this statement (check)
        }

        //if cell empty or available => change the state to the upcoming one
        public void SetState(CellState newState)
        {
            if (IsAvailable())
            {
                _state = newState;
            }
        }

        public void ChangeCellState(CellState newState)
        {
            _state = newState;
        }

        public CellState GetCellState()
        {
            return _state;
        }
    }
}