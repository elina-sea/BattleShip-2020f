using System;

namespace GameEntities
{
    [Serializable]
    public class Cell
    {
        //PK
        public int CellId { get; set; }
        //FK
        public int BoardId { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Cell()
        {
        }
        
        public Cell(CellState state)
        {
            _state = state;
        }

        //TODO move enums to separate folder
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