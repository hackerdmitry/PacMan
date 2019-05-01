using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacManWindow : Form
    {
        readonly Size FrameSize = new Size(17, 39);
        readonly GameController gameController;

        public PacManWindow()
        {
            DoubleBuffered = true;
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Text = "PacMan";

            string[] notParsedMap = new StreamReader("../../Levels/fields_level_1.txt").ReadToEnd()
//            string[] notParsedMap = new StreamReader("../../Levels/demo.txt").ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            char[,] charFields = new char[notParsedMap.Length, notParsedMap[0].Length];
            for (int i = 0; i < charFields.GetLength(0); i++)
                for (int j = 0; j < charFields.GetLength(1); j++)
                    charFields[i, j] = notParsedMap[i][j];
            gameController = new GameController(this, new Map(charFields), GetTimer());
            Width = Map.LENGTH_CELL * gameController.Map.WidthCountCell + FrameSize.Width;
            Height = Map.LENGTH_CELL * gameController.Map.HeightCountCell + FrameSize.Height;
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
            gameController.OnPaint(e);
        }

        void TimerTick(object sender, EventArgs args) { Invalidate(); }
    }
}