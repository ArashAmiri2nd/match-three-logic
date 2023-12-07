using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchThreeLogic
{
    public class NukeTile : MissileTile
    {
        public NukeTile(int x, int y) : base(x, y) { }
        protected override string TileSpecificName() => "n";
        public override void Destroy(Board board)
        {
            var tilesToDestroy = new HashSet<BaseTile>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var adjacentTile = board.GetAdjacentTile(this, direction);
                if (adjacentTile != null)
                    tilesToDestroy.Add(adjacentTile);
            }

            foreach (var tile in tilesToDestroy)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    var adjacentTile = board.GetAdjacentTile(tile, direction);
                    if (adjacentTile != null)
                        tilesToDestroy.Add(adjacentTile);
                }
            }
            
            foreach (var tile in tilesToDestroy)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    var adjacentTile = board.GetAdjacentTile(tile, direction);
                    if (adjacentTile != null)
                        tilesToDestroy.Add(adjacentTile);
                }
            }
            
            board.Destroy(tilesToDestroy.ToList());
        }
    }
}