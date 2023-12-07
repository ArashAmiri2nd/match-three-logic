using System;
using System.Collections.Generic;

namespace MatchThreeLogic
{
    public class GameLogic
    {
        private Board Board { get; set; }
        public Tile[,] Tiles => Board.Tiles;

        public GameLogic(GameSettings settings)
        {
            Board = new Board(settings);
        }

        public void MoveTile(int x, int y, Direction direction)
        {
            var isMovable = Board.IsMoveValid(x, y, direction);

            if (!isMovable)
                return;

            var newPosition = Board.MoveTile(x, y, direction);
            var matchedTilesOriginal = Board.GetMatchedTiles(x, y);
            var matchedTilesMoved = Board.GetMatchedTiles(newPosition.Item1, newPosition.Item2);
            
            if (matchedTilesMoved.Count == 0 && matchedTilesOriginal.Count == 0)
            {
                Board.MoveTile(newPosition.Item1, newPosition.Item2, direction.GetOpposing());
                return;
            }

            Board.DestroyTiles(matchedTilesOriginal);
            Board.DestroyTiles(matchedTilesMoved);
            Board.FillEmptySpaces();
        }

        public Tile GetNewTile(List<Tile> tiles)
        {
            if (tiles.Count == 3)
                return null;

            // if (tiles.Count == 4)
            //     return new MissileTile();
            //
            // if (tiles.Count == 5)
            //     return new BombTile();
            //
            // if (tiles.Count > 5)
            //     return new NukeTile();

            return null;
        }
    }
}