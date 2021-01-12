using System;
using System.Collections.Generic;
using GameEntities;

namespace GameLogic
{
    [Serializable]
    public class GameState
    {
        // Save file
        public GameState()
        {
            
        }
        public GameState(bool currentMoveByPlayerOne, Player playerOne, Player playerTwo)
        {
            CurrentMoveByPlayerOne = currentMoveByPlayerOne;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }

        public bool CurrentMoveByPlayerOne { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }

        //public int Width { get; set; }
        //public int Height { get; set; }
    }
}