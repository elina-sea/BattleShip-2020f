namespace GameEntities
{
    public class Ship
    {
        //PK
        public int ShipId { get; set; }
        public ShipState shipState;
        public ShipType shipType;
        private int _size;
        public bool isVertical;
        private int _xPos;
        private int _yPos;
        
        public enum ShipType 
        {
            Carrier, Battleship, Submarine, Cruiser, Patrol
        }
        public enum ShipState
        {
            Alive, Damaged, Drown
        }

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
            this._size = GetShipSizeByShipType(type);
            this.isVertical = isVertical;
            this.shipState = shipState;
        }
    } 
}