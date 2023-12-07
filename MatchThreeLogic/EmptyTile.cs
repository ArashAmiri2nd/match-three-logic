namespace MatchThreeLogic
{
    public class EmptyTile : BaseTile
    {
        public EmptyTile(int x, int y) : base(x, y)
        {
        }

        protected override bool DoesMatchTileSpecific(BaseTile tile) => false;
        protected override string TileSpecificName() => "-";
        public override bool IsMovable() => true;
        public override void Destroy(Board board) { }
    }
}