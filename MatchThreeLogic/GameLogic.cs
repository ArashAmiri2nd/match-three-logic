namespace MatchThreeLogic
{
    public class GameLogic
    {
        private Board Board { get; }
        private IGameListener GameListener { get; }

        public GameLogic(GameSettings settings, IGameListener gameListener)
        {
            Board = new Board(settings);
            GameListener = gameListener;
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

            Board.MatchTiles(matchedTilesOriginal, x, y);
            Board.MatchTiles(matchedTilesMoved, newPosition.Item1, newPosition.Item2);
            GameListener.OnBoardUpdate(Board);

            Board.FillEmptyTiles();
        }
    }
}