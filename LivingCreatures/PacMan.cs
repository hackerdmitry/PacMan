using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : Creature
    {
        public override float Speed => 1.5f;

        public PacMan(GameController gameController, Position accuratePostion) :
            base(gameController, accuratePostion,
                 Directory.GetFiles("../../Pictures/Player").Select(x => new Bitmap(x)).ToArray()) =>
            GameController.PacManWindow.KeyDown += ChangeDirection;

        public override void Move()
        {
            base.Move();
            GameController.PacManWindow.Text = $@"PacMan {AccuratePosition.x} {AccuratePosition.y}";
        }

        void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    desiredDirection = Direction.Down;
                    break;
                case Keys.D:
                case Keys.Right:
                    desiredDirection = Direction.Right;
                    break;
                case Keys.S:
                case Keys.Down:
                    desiredDirection = Direction.Up;
                    break;
                case Keys.A:
                case Keys.Left:
                    desiredDirection = Direction.Left;
                    break;
            }
        }
    }
}