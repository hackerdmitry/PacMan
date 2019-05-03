using System.Drawing;

namespace PacMan
{
    public class Dot : IDots
    {
        public Bitmap Bitmap { get; }

        public Dot() => Bitmap = new Bitmap("../../Pictures/Dots/Dot.png");
    }
}