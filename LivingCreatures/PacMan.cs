using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : Creature
    {
        public sealed override float Speed { get; protected set; }
        bool isDied;
        int eatedDots;

        static readonly Bitmap[] diedPacman =
            Directory.GetFiles("../../Pictures/Player/Died").Select(x => new Bitmap(x)).ToArray();

        public PacMan(GameController gameController, Position accuratePostion, int eatedDots) :
            base(gameController, accuratePostion,
                 Directory.GetFiles("../../Pictures/Player/Run").Select(x => new Bitmap(x)).ToArray(),
                 50)
        {
            GameController.PacManWindow.KeyDown += ChangeDirection;
            this.eatedDots = eatedDots;
            NormalSpeed();
        }

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
                    return difPos.Length() < STANDART_SPEED * 1.5f;
                })
                .ToList();
            GameController.Map.MapDots.EatDots(eatenDots, GameController);
            eatedDots += eatenDots.Count;
            if (eatenDots.Count != 0)
            {
                GameController.Map.InformFruits(eatedDots);
                
            }
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

        public void SpeedInFear()
        {
            if (GameController.Level == 1) Speed = STANDART_SPEED * 0.9f;
            else if (GameController.Level <= 4) Speed = STANDART_SPEED * 0.95f;
            else if (GameController.Level <= 20) Speed = STANDART_SPEED;
            else NormalSpeed();
        }

        public void NormalSpeed()
        {
            if (GameController.Level == 1) Speed = STANDART_SPEED * 0.8f;
            else if (GameController.Level <= 4) Speed = STANDART_SPEED * 0.9f;
            else if (GameController.Level <= 20) Speed = STANDART_SPEED;
            else Speed = STANDART_SPEED * 0.9f;
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