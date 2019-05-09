using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public abstract class Creature
    {
        protected GameController GameController { get; }
        public Direction CurrentDirection { get; set; }

        public const float STANDART_SPEED = 3f;
        public Position AccuratePosition { get; private set; }
        Position lastAccuratePosition;
        protected Direction desiredDirection;
        protected int iAnimation;
        protected Bitmap[] animation;
        public readonly Timer timer;
        protected bool PlayAnyway;

        protected Creature(GameController gameController, Position accuratePostion, Bitmap[] animation,
                           int intervalAnimation)
        {
            CurrentDirection = Direction.Right;
            desiredDirection = CurrentDirection;
            AccuratePosition = accuratePostion;
            this.animation = animation;
            lastAccuratePosition = AccuratePosition;
            GameController = gameController;
            timer = new Timer();
            timer.Interval = intervalAnimation;
            timer.Tick += (s, e) =>
            {
                if (!(lastAccuratePosition - AccuratePosition).Equals(Position.Empty) || PlayAnyway)
                    iAnimation = (iAnimation + 1) % this.animation.Length;
            };
            timer.Start();
        }

        public abstract float Speed { get; protected set; }

        public virtual Bitmap Bitmap
        {
            get
            {
                if (iAnimation >= animation.Length) iAnimation = 0;
                Bitmap necessaryBitmap = (Bitmap) animation[iAnimation].Clone();
                necessaryBitmap.RotateFlip(GetRotateFlipType(CurrentDirection));
                return necessaryBitmap;
            }
        }

        static RotateFlipType GetRotateFlipType(Direction direction)
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

        public virtual void Move()
        {
            lastAccuratePosition = AccuratePosition;
            AccuratePosition = GameController.Move(this, desiredDirection);
        }
    }
}