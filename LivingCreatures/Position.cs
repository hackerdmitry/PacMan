using System;
using System.Drawing;

namespace PacMan
{
    public class Position
    {
        public readonly int x, y;
        readonly float modX, modY;

        float X => x + modX;
        float Y => y + modY;

        public static readonly Position Empty = new Position(0, 0);

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        Position(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
            modX = x - this.x;
            modY = y - this.y;
        }

        public Position Normalize() => new Position((int) Math.Round((double) x / Map.LENGTH_CELL) * Map.LENGTH_CELL,
                                                    (int) Math.Round((double) y / Map.LENGTH_CELL) * Map.LENGTH_CELL);
        
        public Position Flip() => new Position(Y, X);

        public float Length() => (float) Math.Sqrt(X * X + Y * Y);
        
        public static Position operator +(Position pos1, Position pos2)
            => new Position(pos1.X + pos2.X, pos1.Y + pos2.Y);

        public static Position operator +(Position pos1, Size pos2)
            => new Position(pos1.X + pos2.Width, pos1.Y + pos2.Height);

        public static Position operator +(Position pos1, int num)
            => new Position(pos1.X + num, pos1.Y + num);

        public static Position operator +(Position pos1, float num)
            => new Position(pos1.X + num, pos1.Y + num);

        public static Position operator -(Position pos1, Position pos2)
            => new Position(pos1.X - pos2.X, pos1.Y - pos2.Y);

        public static Position operator *(Position pos1, int num)
            => new Position(pos1.X * num, pos1.Y * num);

        public static Position operator *(Position pos1, float num)
            => new Position(pos1.X * num, pos1.Y * num);

        public static Position operator /(Position pos1, int num)
            => new Position(pos1.X / num, pos1.Y / num);

        public static Position operator %(Position pos1, Size pos2)
            => new Position(pos1.X % pos2.Width, pos1.Y % pos2.Height);

        public override int GetHashCode() => x * 397 ^ y;

        public override bool Equals(object obj)
        {
            Position position = (Position) obj;
            return position.x == x && position.y == y;
        }
    }
}