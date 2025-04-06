using System;

namespace GameEngine.DTO.PathFinderCalculator
{
    public readonly struct Direction
    {
        public int X { get; }
        public int Y { get; }
        public double Cost { get; }

        public Direction(
            int x,
            int y,
            double cost)
        {
            X = x;
            Y = y;
            Cost = cost;
        }

        public static readonly Direction Up = new Direction(0, 1, 1);
        public static readonly Direction Down = new Direction(0, -1, 1);
        public static readonly Direction Left = new Direction(-1, 0, 1);
        public static readonly Direction Right = new Direction(1, 0, 1);
        public static readonly Direction UpRight = new Direction(1, 1, Math.Sqrt(2));
        public static readonly Direction UpLeft = new Direction(-1, 1, Math.Sqrt(2));
        public static readonly Direction DownRight = new Direction(1, -1, Math.Sqrt(2));
        public static readonly Direction DownLeft = new Direction(-1, -1, Math.Sqrt(2));

        public static readonly Direction[] MovementVectors = new Direction[]
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left,
            Direction.UpRight,
            Direction.UpLeft,
            Direction.DownRight,
            Direction.DownLeft
        };
    }
}
