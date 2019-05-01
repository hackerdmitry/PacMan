using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : IControlled
    {
        GameController GameController { get; }

        readonly Bitmap[] animation =
        {
            new Bitmap("../../Pictures/Player/Player1.png"),
            new Bitmap("../../Pictures/Player/Player2.png"),
            new Bitmap("../../Pictures/Player/Player3.png"),
            new Bitmap("../../Pictures/Player/Player2.png")
        };

        int iAnimation;

        public Bitmap Bitmap
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

        public Direction CurrentDirection { get; set; }
        public Position AccuratePosition { get; private set; }
//        public Position PositionRegardingMapCells => 
//            new Position((AccuratePosition.x + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL,
//                         (AccuratePosition.y + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL);

        Direction desiredDirection;

        public PacMan(GameController gameController, Position accuratePostion)
        {
//            //TODO перестать хардкодить
//            Bitmap = new Bitmap("../../Pictures/Player.png");
            CurrentDirection = Direction.Right;
            AccuratePosition = accuratePostion;
            GameController = gameController;
            GameController.PacManWindow.KeyDown += ChangeDirection;
            Timer timer = new Timer();
            timer.Interval = 32;
            timer.Tick += ChangeAnimation;
            timer.Start();
        }

        void ChangeAnimation(object sender, EventArgs e) => iAnimation = (iAnimation + 1) % animation.Length;

        public void Move()
        {
            AccuratePosition = GameController.Move(this, desiredDirection);
            GameController.PacManWindow.Text = $"PacMan {AccuratePosition.x} {AccuratePosition.y}";
        }

        void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    desiredDirection = Direction.Down;
                    break;
                case Keys.D:
                    desiredDirection = Direction.Right;
                    break;
                case Keys.S:
                    desiredDirection = Direction.Up;
                    break;
                case Keys.A:
                    desiredDirection = Direction.Left;
                    break;
            }
        }
    }
}