using System;
using MatchThreeLogic;

namespace TestConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var settings = new GameSettings(5, 5, 3,Direction.Down);

            var logic = new GameLogic(settings);
            DrawBoard(logic.Tiles);
            ReceiveInput(logic);
        }

        private static void ReceiveInput(GameLogic logic)
        {
            var input = Console.ReadLine();
            var x = int.Parse(input[0].ToString());
            var y = int.Parse(input[1].ToString());
            Direction direction;
            switch (input[2])
            {
                case 'u':
                    direction = Direction.Up;
                    break;
                case 'd':
                default:
                    direction = Direction.Down;
                    break;
                case 'l':
                    direction = Direction.Left;
                    break;
                case 'r':
                    direction = Direction.Right;
                    break;
            }
            
            logic.MoveTile(x, y, direction);
            DrawBoard(logic.Tiles);
        }

        private static void DrawBoard(BaseTile[,] board)
        {
            var drawString = "";

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    drawString += (board[i, j] == null ? "-1" : board[i, j].ToString()) + " ";
                }

                drawString += "\n";
            }

            Console.WriteLine(drawString);
        }
    }
}