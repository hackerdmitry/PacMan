using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public abstract class Creature
    {
        protected GameController GameController { get; }
        public Direction CurrentDirection { get; set; }

        public Position AccuratePosition { get; private set; }
        Position lastAccuratePosition;
        protected Direction desiredDirection;
        protected int iAnimation;
        protected readonly Bitmap[] animation;

        public Creature(GameController gameController, Position accuratePostion, Bitmap[] animation)
        {
            CurrentDirection = Direction.Right;
            desiredDirection = CurrentDirection;
            AccuratePosition = accuratePostion;
            this.animation = animation;
            lastAccuratePosition = AccuratePosition;
            GameController = gameController;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, e) =>
            {
                if (!(lastAccuratePosition - AccuratePosition).Equals(Position.Empty))
                    iAnimation = (iAnimation + 1) % animation.Length;
            };
            timer.Start();
        }

        public abstract float Speed { get; }

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
//        Position PositionRegardingMapCells { get; }

        public virtual void Move()
        {
            lastAccuratePosition = AccuratePosition;
            AccuratePosition = GameController.Move(this, desiredDirection);
        }
    }
}