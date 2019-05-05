using System.Windows.Forms;

namespace PacMan
{
    public class Score
    {
        public int Height { get; }
        readonly GameController gameController;

        int score;
        
        public Score(GameController gameController)
        {
            this.gameController = gameController;
            Height = Map.LENGTH_CELL * 3;
        }

        public void AddScore(int score) => this.score += score;

        public void OnPaint(PaintEventArgs e)
        {
            
        }
    }
}