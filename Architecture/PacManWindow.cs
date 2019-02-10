using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class PacManWindow : Form
    {
        Bitmap bitmap = new Bitmap(Image.FromFile("../../Pictures/field1.png"));
        int x, y;
        Graphics g;

        public PacManWindow()
        {
            Timer timer = new Timer
            {
                Interval = 32,
                Enabled = true
            };
            timer.Tick += TimerTick;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (g == null) g = e.Graphics;
            Text = x.ToString();
            base.OnPaint(e);  
            e.Graphics.DrawImage(bitmap, new Point(x++, y++));
            e.Graphics.ResetTransform();
        }

        void TimerTick(object sender, EventArgs args)
        {
            Text = x++.ToString();
            Invalidate();
//            g?.DrawImage(bitmap, new Point(x++, y++));
//            g?.ResetTransform();
        }
    }
}