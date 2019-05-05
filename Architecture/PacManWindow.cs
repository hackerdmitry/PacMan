using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacManWindow : Form
    {
        readonly Size frameSize = new Size(17, 39);
        public GameController GameController { get; private set; }
        public readonly string FolderLevelPath;

        public PacManWindow(string folderLevelPath)
        {
            FolderLevelPath = folderLevelPath;
            StartGame();
        }

        void StartGame()
        {
            DoubleBuffered = true;
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Text = "PacMan";

            string[] notParsedFields = new StreamReader($"{FolderLevelPath}/fields.txt").ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string[] notParsedDots = new StreamReader($"{FolderLevelPath}/dots.txt").ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            char[,] charFields = new char[notParsedFields.Length, notParsedFields[0].Length];
            char[,] charDots = new char[notParsedDots.Length, notParsedDots[0].Length];
            for (int i = 0; i < charFields.GetLength(0); i++)
                for (int j = 0; j < charFields.GetLength(1); j++)
                {
                    charFields[i, j] = notParsedFields[i][j];
                    charDots[i, j] = notParsedDots[i][j];
                }
            GameController = new GameController(this, new Map(charFields, charDots, this), GetTimer());
            Width = Map.LENGTH_CELL * GameController.Map.WidthCountCell + frameSize.Width;
            Height = Map.LENGTH_CELL * GameController.Map.HeightCountCell + frameSize.Height +
                     GameController.Score.Height + GameController.Footer.Height;
            StartPosition = FormStartPosition.CenterScreen;
        }

        Timer GetTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 32 * Map.DEFAULT_LENGTH_CELL / Map.LENGTH_CELL;
            timer.Enabled = true;
            timer.Tick += TimerTick;
            return timer;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GameController.OnPaint(e);
        }

        void TimerTick(object sender, EventArgs args) { Invalidate(); }

        public void Restart()
        {
            GameController.Dispose();
            Program.restart = true;
            Close();
        }
    }
}