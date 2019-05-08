using System.Drawing;

namespace PacMan
{
    public class SpawnLine : IField
    {
        public SpawnLine(int x, int y)
        {
            X = x;
            Y = y;
            Bitmap = new Bitmap("../../Pictures/SpawnWalls/WallToLine.png");
        }

        public int X { get; }
        public int Y { get; }
        public bool IsWall(Creature creature) => creature is PacMan;
        public Bitmap Bitmap { get; }
    }
}