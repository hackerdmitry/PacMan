using System.Drawing;

namespace PacMan
{
    public class Dot : IDots
    {
        public int Value => 10;

        public Bitmap Bitmap { get; }
        public void Act(GameController gameController) { }

        public Dot() => Bitmap = new Bitmap("../../Pictures/Dots/Dot.png");
    }
}