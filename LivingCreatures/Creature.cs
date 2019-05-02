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

        public abstract Bitmap Bitmap { get; }
//        Position PositionRegardingMapCells { get; }

        public virtual void Move()
        {
            lastAccuratePosition = AccuratePosition;
            AccuratePosition = GameController.Move(this, desiredDirection);
        }
    }
}