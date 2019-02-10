using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class PacManWindow : Form
    {
        Bitmap bitmap = new Bitmap(Image.FromFile("../../Pictures/field1.png"));
        
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, new Point(0, 0));
        }
    }
}