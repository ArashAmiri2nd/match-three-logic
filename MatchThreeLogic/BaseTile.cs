namespace MatchThreeLogic
{
    public abstract class BaseTile
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        protected BaseTile(int x, int y)
        {
            SetPosition(x, y);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public abstract void Destroy(Board board);

        public bool DoesMatch(BaseTile tile)
        {
            if (tile == null)
                return false;

            return DoesMatchTileSpecific(tile);
        }

        protected abstract bool DoesMatchTileSpecific(BaseTile tile);
        protected abstract string TileSpecificName();
        public virtual void AvoidImmediateMatch(int totalColors){}
        public abstract bool IsMovable();
        public override string ToString() => TileSpecificName();
    }
}