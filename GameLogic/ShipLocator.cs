using GameEntities;
namespace GameLogic 
{
    public class ShipLocator 
    {
        public int X, Y;
        public Ship ship;
        public Player.AvailableShip AvailableShip = null!;
        public ShipLocator(int x, int y, Ship ship)
        {
            this.X = x;
            this.Y = y;
            this.ship = ship;
        }
    }
}