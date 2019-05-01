using System.Drawing;

namespace PacMan
{
    public class Position
    {
        public readonly int x, y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Position operator +(Position pos1, Position pos2)
            => new Position(pos1.x + pos2.x, pos1.y + pos2.y);

        public static Position operator +(Position pos1, Size pos2)
            => new Position(pos1.x + pos2.Width, pos1.y + pos2.Height);

        public static Position operator +(Position pos1, int num)
            => new Position(pos1.x + num, pos1.y + num);

        public static Position operator -(Position pos1, Position pos2)
            => new Position(pos1.x - pos2.x, pos1.y - pos2.y);

        public static Position operator *(Position pos1, int num)
            => new Position(pos1.x * num, pos1.y * num);

        public static Position operator /(Position pos1, int num)
            => new Position(pos1.x / num, pos1.y / num);

        public static Position operator %(Position pos1, Size pos2)
            => new Position(pos1.x % pos2.Width, pos1.y % pos2.Height);
    }
}