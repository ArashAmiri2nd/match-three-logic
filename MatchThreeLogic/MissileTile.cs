using System;
using System.Collections.Generic;

namespace MatchThreeLogic
{
    public class MissileTile : BaseTile
    {
        public MissileTile(int x, int y) : base(x, y) { }
        protected override bool DoesMatchTileSpecific(BaseTile tile) => true;
        protected override string TileSpecificName() => "m";
        public override bool IsMovable() => true;

        public override void Destroy(Board board)
        {
            var tilesToDestroy = new List<BaseTile>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var adjacentTile = board.GetAdjacentTile(this, direction);
                if (adjacentTile != null)
                    tilesToDestroy.Add(adjacentTile);
            }
            board.Destroy(tilesToDestroy);
        }
    }
}