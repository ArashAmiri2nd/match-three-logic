using System;

namespace MatchThreeLogic
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public static class Extensions
    {
        public static Direction GetOpposing(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new Exception();
            }
        }
    }
}