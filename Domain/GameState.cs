using System;
using GameEntities;

namespace Domain
{
    [Serializable]
    public class GameState
    {
        //PK
        //public int GameStateId { get; set; }
        public bool CurrentMoveByPlayerOne { get; set; }
        public Player PlayerOne { get; set; } = null!;
        public Player PlayerTwo { get; set; } = null!;

        public GameState()
        {
        }

        public GameState(bool currentMoveByPlayerOne, Player playerOne, Player playerTwo)
        {
            CurrentMoveByPlayerOne = currentMoveByPlayerOne;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }
    }
}