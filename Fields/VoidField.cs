using System.Drawing;

namespace PacMan
{
    public class VoidField : IField
    {
        public VoidField(int x, int y)
        {
            X = x;
            Y = y;
            Bitmap = new Bitmap("../../Pictures/Fields/VoidField.png");
        }

        public int X { get; }
        public int Y { get; }
        public bool IsWall(Creature creature) => false;
        public Bitmap Bitmap { get; }
    }
}