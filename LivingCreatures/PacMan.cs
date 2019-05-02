using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : Creature
    {
        public override float Speed => 1.5f;

        public override Bitmap Bitmap
        {
            get
            {
                Bitmap necessaryBitmap = (Bitmap) animation[iAnimation].Clone();
                necessaryBitmap.RotateFlip(GetRotateFlipType(CurrentDirection));
                return necessaryBitmap;
            }
        }

        public static RotateFlipType GetRotateFlipType(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return RotateFlipType.Rotate90FlipNone;
                case Direction.Right:
                    return RotateFlipType.RotateNoneFlipNone;
                case Direction.Down:
                    return RotateFlipType.Rotate270FlipNone;
                case Direction.Left:
                    return RotateFlipType.Rotate180FlipY;
            }
            throw new ArgumentException();
        }

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