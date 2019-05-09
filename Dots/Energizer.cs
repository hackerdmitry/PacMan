using System.Drawing;

namespace PacMan
{
    public class Energizer : IDots
    {
        public int Value => 50;
        
        public Bitmap Bitmap { get; }

        public void Act(GameController gameController)
        {
            gameController.ToBlueFearGhosts();
        }

        public Energizer() => Bitmap = new Bitmap("../../Pictures/Dots/Energizer.png");
    }
}