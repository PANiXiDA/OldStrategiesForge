namespace GameEngine.DTO.PathFinderCalculator
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsWalkable { get; set; }

        public Tile(int x, int y, bool isWalkable)
        {
            X = x;
            Y = y;
            IsWalkable = isWalkable;
        }
    }
}
