namespace MatchThreeLogic
{
    public class GameSettings
    {
        public GameSettings(int width, int height, int numberOfTileColors,Direction gravity)
        {
            Width = width;
            Height = height;
            NumberOfTileColors = numberOfTileColors;
            Gravity = gravity;
        }

        public int Width { get; }
        public int Height { get; }
        public int NumberOfTileColors { get; }
        public Direction Gravity { get; }
    }
}
