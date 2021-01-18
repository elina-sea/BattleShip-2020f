using System;
using System.ComponentModel.DataAnnotations;

namespace GameEntities
{
    public class SavedGameData
    {
        public int SavedGameDataId { get; set; }
        public string JsonData { get; set; } = null!;

        public string TimeStamp { get; set; } = null!;
    }
}