using System.Collections.Generic;

namespace MatchThreeLogic
{
    // public abstract class BaseTile
    // {
    //     public int X { get; private set; }
    //     public int Y { get; private set; }
    //     protected Board Board { get; set; }
    //
    //     protected BaseTile(int x, int y, Board board)
    //     {
    //         Board = board;
    //         SetPosition(x, y);
    //     }
    //
    //     protected void SetPosition(int x, int y)
    //     {
    //         X = x;
    //         Y = y;
    //     }
    //
    //     public abstract bool MoveTile(Direction direction);
    // }
    //
    // public class NormalTile : BaseTile
    // {
    //     public int Type { get; private set; }
    //
    //     public NormalTile(int x, int y, Board board, int type) : base(x, y, board)
    //     {
    //         Type = type;
    //     }
    //
    //     public override bool MoveTile(Direction direction)
    //     {
    //         
    //         return true;
    //     }
    // }

    // public class MissileTile : BaseTile
    // {
    //     public MissileTile(int x, int y) : base(x, y) { }
    // }

    public class Tile
    {
        public int Type { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(int type, int x, int y)
        {
            Type = type;
            SetPosition(x, y);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Destroy()
        {
            Type = 9;
        }

        public bool DoesMatch(Tile tile)
        {
            if (tile == null)
                return false;

            return Type == tile.Type;
        }

        public bool IsMovable() => true;

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}