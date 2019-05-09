using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : Creature
    {
        public override float Speed { get; protected set; } = 3f;
        bool isDied;

        static readonly Bitmap[] diedPacman =
            Directory.GetFiles("../../Pictures/Player/Died").Select(x => new Bitmap(x)).ToArray();

        public PacMan(GameController gameController, Position accuratePostion) :
            base(gameController, accuratePostion,
                 Directory.GetFiles("../../Pictures/Player/Run").Select(x => new Bitmap(x)).ToArray(),
                 50) =>
            GameController.PacManWindow.KeyDown += ChangeDirection;

        public override Bitmap Bitmap
        {
            get
            {
                if (!isDied) return base.Bitmap;
                if (iAnimation == diedPacman.Length - 1)
                {
                    isDied = false;
                    GameController.Restart();
                    GameController.Footer.LoseHealth();
                    if (GameController.Footer.GetHealth() <= 0)
                    {
                        GameController.StopTimer();
                        GameController.PacManWindow.Close();
                    }
                }
                return animation[iAnimation];
            }
        }

        public override void Move()
        {
            base.Move();
            List<KeyValuePair<Position, IDots>> eatenDots = GameController.Map.MapDots
                .Where(x =>
                {
                    Position difPos = AccuratePosition - x.Key;
                    return difPos.Length() < Speed;
                })
                .ToList();
            GameController.Map.MapDots.EatDots(eatenDots);
            eatenDots.ForEach(x =>
            {
                GameController.Score.AddScore(x.Value.Value);
                x.Value.Act(GameController);
            });
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

        public void Die()
        {
            animation = diedPacman;
            isDied = true;
            PlayAnyway = true;
            timer.Interval = 100;
        }
    }
}