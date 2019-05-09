using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class Footer
    {
        public int Height { get; }
        readonly GameController gameController;
        readonly Bitmap lifePlayer = new Bitmap("../../Pictures/Player/Life/Life.png");

        int countHealth = 3;

        public Footer(GameController gameController)
        {
            this.gameController = gameController;
            Height = Map.LENGTH_CELL * 2;
        }

        public int GetHealth() => countHealth;

        public void LoseHealth() => countHealth--;
        
        public void AddHealth() => countHealth++;

        public void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < countHealth; i++)
                e.Graphics.DrawImage(lifePlayer,
                                     new Rectangle((int) ((2.375f + i * 1.5f) * Map.LENGTH_CELL),
                                                   (int) (25.6f * Map.LENGTH_CELL),
                                                   Map.LENGTH_CELL, Map.LENGTH_CELL));
        }
    }
}