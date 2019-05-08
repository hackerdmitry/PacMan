using System.Drawing;

namespace PacMan
{
    public class SimpleField : IField
    {
        public SimpleField(int x, int y)
        {
            X = x;
            Y = y;
            Bitmap = new Bitmap("../../Pictures/Fields/SimpleField.png");
        }

        public int X { get; }
        public int Y { get; }
        public bool IsWall(Creature creature) => false;
        public Bitmap Bitmap { get; }
    }
}