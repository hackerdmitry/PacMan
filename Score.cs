using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PacMan
{
    public class Score
    {
        public int Height { get; }
        readonly GameController gameController;

        readonly Brush brush = new SolidBrush(Color.Azure);
        readonly Font font = new Font("Joystix", Map.LENGTH_CELL * 0.825f);
        int score;
        const string NAME_FILE = "highScore";
        int highScore;

        public Score(GameController gameController)
        {
            if (File.Exists(NAME_FILE))
                using (StreamReader streamReader = new StreamReader(NAME_FILE))
                    highScore = int.Parse(streamReader.ReadToEnd());
            else
                using (StreamWriter streamWriter = new StreamWriter(NAME_FILE))
                    streamWriter.Write(0);
            this.gameController = gameController;
            Height = Map.LENGTH_CELL * 3;
        }

        public void AddScore(int score)
        {
            for (int i = 0; i < (this.score + score) / 10000 - this.score / 10000; i++)
                gameController.Footer.AddHealth();
            this.score += score;
            if (this.score > highScore)
            {
                highScore = this.score;
                using (StreamWriter streamWriter = new StreamWriter(NAME_FILE))
                    streamWriter.Write(highScore);
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("1UP", font, brush,
                                  Map.LENGTH_CELL * 3.25f, 0);
            e.Graphics.DrawString(score.ToString(), font, brush,
                                  Map.LENGTH_CELL * (6.25f - score.ToString().Length), Map.LENGTH_CELL);
            e.Graphics.DrawString("HIGH SCORE", font, brush,
                                  Map.LENGTH_CELL * 9.125f, 0);
            e.Graphics.DrawString(highScore.ToString(), font, brush,
                                  Map.LENGTH_CELL * (16.125f - highScore.ToString().Length), Map.LENGTH_CELL);
            string gameControllerLevel = gameController.Level + " LVL";
            e.Graphics.DrawString(gameControllerLevel, font, brush,
                                  Map.LENGTH_CELL * (24f - gameControllerLevel.Length), Map.LENGTH_CELL);
        }
    }
}