using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchThreeLogic
{
    public class Board
    {
        public BaseTile[,] Tiles { get; private set; }
        private readonly GameSettings _settings;

        public event Action OnUpdated;

        public Board(GameSettings settings)
        {
            _settings = settings;
            InitializeRandomBoard();
        }

        private void InitializeRandomBoard()
        {
            Tiles = new BaseTile[_settings.Width, _settings.Height];
            RandomizeBoard();
            ResolveImmediateMatches();
        }

        private void RandomizeBoard()
        {
            var random = new Random();
            for (var i = 0; i < Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j] = new NormalTile(i, j, random.Next(0, _settings.NumberOfTileColors));
                }
            }
        }

        private void ResolveImmediateMatches()
        {
            for (var i = 0; i < Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < Tiles.GetLength(1); j++)
                {
                    if (GetMatchedTiles(Tiles[i, j]).Count != 0)
                    {
                        Tiles[i, j].AvoidImmediateMatch(_settings.NumberOfTileColors);
                        ResolveImmediateMatches();
                        return;
                    }
                }
            }
        }

        private List<List<BaseTile>> GetAllMatchedTiles()
        {
            var matches = new List<List<BaseTile>>();
            foreach (var tile in Tiles)
            {
                var matchedTiles = GetMatchedTiles(tile);
                if (matchedTiles.Count == 0)
                    continue;

                var hashsetMatches = new HashSet<BaseTile>(matchedTiles);
                if (!matches.Any(m => hashsetMatches.SetEquals(new HashSet<BaseTile>(m))))
                    matches.Add(matchedTiles);
            }

            //todo => remove duplicates here
            return matches;
        }

        private List<BaseTile> GetMatchedTiles(BaseTile tile)
        {
            var checkForMatchesMap = new Dictionary<BaseTile, bool> { { tile, false } };
            var finalTiles = new List<BaseTile>();

            do
            {
                var tileToCheck = checkForMatchesMap.FirstOrDefault(t => !t.Value).Key;
                if (tileToCheck == null)
                    break;

                var (possibleMatches, definiteMatches) = CheckTwoDimensions(tileToCheck);
                checkForMatchesMap[tileToCheck] = true;

                foreach (var possibleTile in possibleMatches)
                {
                    if (!checkForMatchesMap.ContainsKey(possibleTile))
                        checkForMatchesMap.Add(possibleTile, false);
                }

                foreach (var definiteTile in definiteMatches)
                {
                    if (!finalTiles.Contains(definiteTile))
                        finalTiles.Add(definiteTile);
                }
            } while (checkForMatchesMap.ContainsValue(false));

            return finalTiles.Contains(tile) ? finalTiles : new List<BaseTile>();

            (List<BaseTile> possibleMatches, List<BaseTile> definiteMatches) CheckTwoDimensions(BaseTile tileToCheck)
            {
                var possible = new HashSet<BaseTile>();
                var definite = new HashSet<BaseTile>();

                var topTile = GetAdjacentTile(tileToCheck, Direction.Up);
                if (tile.DoesMatch(topTile))
                    possible.Add(topTile);

                var bottomTile = GetAdjacentTile(tileToCheck, Direction.Down);
                if (tile.DoesMatch(bottomTile))
                    possible.Add(bottomTile);

                if (tileToCheck.DoesMatch(topTile) && tileToCheck.DoesMatch(bottomTile))
                {
                    definite.Add(tileToCheck);
                    definite.Add(topTile);
                    definite.Add(bottomTile);
                }

                var leftTile = GetAdjacentTile(tileToCheck, Direction.Left);
                if (tile.DoesMatch(leftTile))
                    possible.Add(leftTile);

                var rightTile = GetAdjacentTile(tileToCheck, Direction.Right);
                if (tile.DoesMatch(rightTile))
                    possible.Add(rightTile);

                if (tileToCheck.DoesMatch(rightTile) && tileToCheck.DoesMatch(leftTile))
                {
                    definite.Add(tileToCheck);
                    definite.Add(rightTile);
                    definite.Add(leftTile);
                }

                return (possible.ToList(), definite.ToList());
            }
        }

        public List<BaseTile> GetMatchedTiles(int x, int y)
        {
            return GetMatchedTiles(Tiles[x, y]);
        }

        public BaseTile GetAdjacentTile(BaseTile tile, Direction direction)
        {
            if (tile == null)
                return null;

            if (tile.X == 0 && direction is Direction.Up)
                return null;

            if (tile.Y == 0 && direction is Direction.Left)
                return null;

            var rowsCount = Tiles.GetLength(0);
            var columnCount = Tiles.GetLength(1);
            if (tile.X == rowsCount - 1 && direction is Direction.Down)
                return null;

            if (tile.Y == columnCount - 1 && direction is Direction.Right)
                return null;

            switch (direction)
            {
                case Direction.Down:
                default:
                    return Tiles[tile.X + 1, tile.Y];
                case Direction.Up:
                    return Tiles[tile.X - 1, tile.Y];
                case Direction.Right:
                    return Tiles[tile.X, tile.Y + 1];
                case Direction.Left:
                    return Tiles[tile.X, tile.Y - 1];
            }
        }

        public void MatchTiles(List<BaseTile> matchedTiles, int sourceX, int sourceY)
        {
            if (matchedTiles.Count == 0)
                return;

            foreach (var tile in matchedTiles)
                tile.Destroy(this);

            Tiles[sourceX, sourceY] = GetNewTileAfterMatch(matchedTiles, sourceX, sourceY);
        }

        public void Destroy(List<BaseTile> matchedTiles)
        {
            foreach (var tile in matchedTiles)
                tile.Destroy(this);
        }

        private BaseTile GetNewTileAfterMatch(List<BaseTile> matchedTiles, int x, int y)
        {
            if (matchedTiles.Count == 3)
                return new EmptyTile(x, y);

            if (matchedTiles.Count == 4)
                return new MissileTile(x, y);

            if (matchedTiles.Count == 5)
                return new BombTile(x, y);

            if (matchedTiles.Count > 5)
                return new NukeTile(x, y);

            throw new Exception("matched tiles is less than 3 which means something went wrong");
        }

        public (int, int) MoveTile(int x, int y, Direction direction)
        {
            var targetTile = GetAdjacentTile(Tiles[x, y], direction);
            var currentTile = Tiles[x, y];

            var targetPosition = (targetTile.X, targetTile.Y);

            Tiles[x, y] = targetTile;
            Tiles[targetTile.X, targetTile.Y] = currentTile;

            targetTile.SetPosition(x, y);
            currentTile.SetPosition(targetPosition.X, targetPosition.Y);

            return targetPosition;
        }

        public bool IsMoveValid(int x, int y, Direction direction)
        {
            var adjacentTile = GetAdjacentTile(Tiles[x, y], direction);
            return adjacentTile != null && adjacentTile.IsMovable();
        }

        public void FillEmptyTiles()
        {
            var emptyTiles = GetEmptyTiles();

            foreach (var emptyTile in emptyTiles)
            {
                while (IsMoveValid(emptyTile.X, emptyTile.Y, _settings.Gravity.GetOpposing()))
                    MoveTile(emptyTile.X, emptyTile.Y, _settings.Gravity.GetOpposing());
            }

            var random = new Random();
            emptyTiles = GetEmptyTiles();
            foreach (var emptyTile in emptyTiles)
            {
                Tiles[emptyTile.X, emptyTile.Y] = new NormalTile(emptyTile.X, emptyTile.Y,
                    random.Next(0, _settings.NumberOfTileColors));
            }

            OnUpdated?.Invoke();

            while (GetAllMatchedTiles().Count != 0)
            {
                var allMatchedTiles = GetAllMatchedTiles();

                Console.WriteLine(allMatchedTiles.Count);
                foreach (var matchedTiles in allMatchedTiles)
                    MatchTiles(matchedTiles, matchedTiles.First().X, matchedTiles.First().Y);

                OnUpdated?.Invoke();
            }
        }

        private List<BaseTile> GetEmptyTiles()
        {
            var emptyTiles = new List<BaseTile>();

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    if (Tiles[i, j] is EmptyTile)
                        emptyTiles.Add(Tiles[i, j]);
                }
            }

            return emptyTiles;
        }
    }
}