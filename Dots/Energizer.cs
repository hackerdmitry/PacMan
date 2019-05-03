using System.Drawing;

namespace PacMan
{
    public class Energizer : IDots
    {
        public Bitmap Bitmap { get; }

        public Energizer() => Bitmap = new Bitmap("../../Pictures/Dots/Energizer.png");
    }
}