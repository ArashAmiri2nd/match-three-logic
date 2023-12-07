using System.Collections.Generic;

namespace MatchThreeLogic
{
    public class NormalTile : BaseTile
    {
        public int Type { get; private set; }
        
        public NormalTile(int x, int y, int type) : base(x, y)
        {
            Type = type;
        }

        protected override bool DoesMatchTileSpecific(BaseTile tile)
        {
            if (tile is NormalTile normalTile)
                return normalTile.Type == Type;

            return false;
        }

        protected override string TileSpecificName() => Type.ToString();

        public override void AvoidImmediateMatch(int totalColors)
        {
            Type = (Type + 1) % totalColors;
        }

        public override bool IsMovable() => true;
        
        public override void Destroy(Board board)
        {
            board.Tiles[X, Y] = new EmptyTile(X, Y);
        }
    }
}