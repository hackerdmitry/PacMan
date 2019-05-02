using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : IControlled
    {
        GameController GameController { get; }

        readonly Bitmap[] animation;

        public float Speed => 1.5f;
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
        Position lastAccuratePosition;
        public Position AccuratePosition { get; private set; }
//        public Position PositionRegardingMapCells => 
//            new Position((AccuratePosition.x + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL,
//                         (AccuratePosition.y + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL);

        Direction desiredDirection;

        public PacMan(GameController gameController, Position accuratePostion)
        {
            CurrentDirection = Direction.Right;
            AccuratePosition = accuratePostion;
            lastAccuratePosition = AccuratePosition;
            animation = Directory.GetFiles("../../Pictures/Player").Select(x => new Bitmap(x)).ToArray();
            GameController = gameController;
            GameController.PacManWindow.KeyDown += ChangeDirection;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) =>
            {
                if (!(lastAccuratePosition - AccuratePosition).Equals(Position.Empty))
                    iAnimation = (iAnimation + 1) % animation.Length;
            };
            timer.Start();
        }

        public void Move()
        {
            lastAccuratePosition = AccuratePosition;
            AccuratePosition = GameController.Move(this, desiredDirection);
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