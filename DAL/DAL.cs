using GameEntities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /*
      <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.9" />
      <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="3.1.9" />
      <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.9" />
     */

    public class AppDbContext : DbContext
    {
        /*public DbSet<Player> Players { get; set; } = null!;
        public DbSet<GameState> GameStates { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;*/

        public DbSet<SavedGameData> SavedGameDataEntries { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(@"
                Server=barrel.itcollege.ee,1533;
                User Id=student;
                Password=Student.Bad.password.0;
                Database=elserg_battleship;
                MultipleActiveResultSets=true;"
                );
            //.UseSqlite("Data Source=/Users/elina_sea/RiderProjects/BattleShip-Game/BattleDB.db");
        }
    }
}