namespace Battleship
{
    class Ship
    {
        public static List<Ship> ships = new();
        public string Name;
        public string Symbol
        {
            get { return Name[0].ToString().ToUpper(); }
        }
        public int Size;
        public int Hits;
        public bool Sunk
        {
            get { return Hits >= Size; }
        }
        public bool Announced;
        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Hits = 0;
            ships.Add(this);
        }
    }
}
