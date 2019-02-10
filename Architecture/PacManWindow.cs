using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacManWindow : Form
    {
        readonly Size FrameSize = new Size(18, 40);
        Map map;

        public PacManWindow()
        {
            DoubleBuffered = true;
            BackColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Text = "PacMan";
            
            string[] notParsedMap = new StreamReader("../../Levels/fields_level_1.txt").ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            char[,] charFields = new char[notParsedMap.Length,notParsedMap[0].Length];
            for (int i = 0; i < charFields.GetLength(0); i++)
                for (int j = 0; j < charFields.GetLength(1); j++)
                    charFields[i, j] = notParsedMap[i][j];
            map = new Map(charFields);
            Width = Map.LENGTH_SQUARE * map.Width + FrameSize.Width;
            Height = Map.LENGTH_SQUARE * map.Height + FrameSize.Height;
            Timer timer = new Timer
            {
                Interval = 16,
                Enabled = true
            };
            timer.Tick += TimerTick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            map.OnPaint(e);
        }

        void TimerTick(object sender, EventArgs args)
        {
            Invalidate();
        }
    }
}