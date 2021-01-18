using System;
using System.ComponentModel.DataAnnotations;

namespace GameEntities
{
    public class SavedGameData
    {
        [Key]
        public int SavedGameDataId { get; set; }
        //[MaxLength(2048)]
        public string JsonData { get; set; } = null!;

        public string TimeStamp { get; set; } = null!;
    }
}