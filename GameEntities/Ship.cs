namespace GameEntities
{
    public class Ship
    {
        public enum ShipType 
        {
            Carrier, Battleship, Submarine, Cruiser, Patrol
        }
        public enum ShipState
        {
            Alive, Damaged, Drown
        }

        public ShipState shipState;
        public ShipType shipType;
        private int size;
        public bool isVertical;
        private int xPos;
        private int yPos;

        public static int GetShipSizeByShipType(ShipType type) //common parameter for all instances
        {
            switch(type)
            {
                case ShipType.Carrier:
                    return 5;
                    break;
                case ShipType.Battleship:
                    return 4;
                    break;
                case ShipType.Submarine:
                    return 3;
                    break;
                case ShipType.Cruiser:
                    return 2;
                    break;
                case ShipType.Patrol:
                    return 1;
                    break;
            }
            return 0;
        }
        
        public Ship(ShipType type, bool isVertical, ShipState shipState)
        {
            this.size = GetShipSizeByShipType(type);
            this.isVertical = isVertical;
            this.shipState = shipState;
        }
    } 
}