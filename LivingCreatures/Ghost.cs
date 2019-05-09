using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class Ghost : Creature
    {
        public sealed override float Speed { get; protected set; } = STANDART_SPEED;
        public static float TimeInBlueFear { get; private set; } = 7f;
        public static float SpeedInBlueFear => 7f;
        public static float TimeToReminderEndBlueFear { get; private set; } = 5f;

        readonly float[] flashesBeforeBlueTime =
        {
            5, 5, 5, 5, 5, 5, 5, 5, 3, 5, 5, 3, 3, 5, 3, 3, 0, 3, 0
        };

        readonly float[] ghostBlueTime =
        {
            6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0
        };

        protected readonly Map Map;
        protected Position Target;

        bool toBase;

        static long startTicksBlueFear;
        protected Bitmap[] OldAnimations;

        const int START_COMBO_SCORE = 100;
        static int comboScore = START_COMBO_SCORE;
        static int countGhostsInBlueFear;

        readonly Brush brush = new SolidBrush(Color.FromArgb(0x31, 0xFF, 0xFF));
        readonly Font font = new Font("Joystix", Map.LENGTH_CELL * 0.333f);

        long startScoreSpawn, durationScore;
        int score;
        int scoreX, scoreY;

        public static int ComboScore
        {
            get
            {
                comboScore *= 2;
                return comboScore;
            }
        }

        static readonly Bitmap[] blueFearAnimations =
            Directory.GetFiles("../../Pictures/Ghost/BlueFear").Select(x => new Bitmap(x)).ToArray();

        static readonly Bitmap[] reminderEndBlueFearAnimations =
            Directory.GetFiles("../../Pictures/Ghost/BlueFearEnd").Select(x => new Bitmap(x)).ToArray();

        static readonly Bitmap[][] animationsDied =
        {
            Directory.GetFiles("../../Pictures/Ghost/Died/Down").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/Ghost/Died/Right").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/Ghost/Died/Up").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/Ghost/Died/Left").Select(x => new Bitmap(x)).ToArray()
        };

        protected Ghost(GameController gameController, Position accuratePostion, Bitmap[] animation, Map map) : base(
            gameController, accuratePostion, animation, 100)
        {
            Map = map;
            Target = new Position(0, 5);
            int iBlueTime = Math.Min(gameController.Level, ghostBlueTime.Length) - 1;
            TimeInBlueFear = ghostBlueTime[iBlueTime] + flashesBeforeBlueTime[iBlueTime];
            TimeToReminderEndBlueFear = flashesBeforeBlueTime[iBlueTime];
            NormalSpeed();
        }

        public override Bitmap Bitmap
        {
            get
            {
                if (iAnimation >= animation.Length) iAnimation = 0;
                return animation[iAnimation];
            }
        }

        public void ToBlueFear()
        {
            if (toBase) return;
            countGhostsInBlueFear++;
            GameController.Player.SpeedInFear();
            startTicksBlueFear = DateTime.Now.Ticks;
            OldAnimations = animation;
            animation = blueFearAnimations;
            SpeedInFear();
        }

        void ToReminderEndBlueFear()
        {
            if (toBase) return;
            animation = reminderEndBlueFearAnimations;
        }

        void SpeedInFear()
        {
            if (GameController.Level == 1) Speed = STANDART_SPEED * 0.5f;
            else if (GameController.Level <= 4) Speed = STANDART_SPEED * 0.55f;
            else if (GameController.Level <= 20) Speed = STANDART_SPEED * 0.6f;
            else NormalSpeed();
        }

        void NormalSpeed()
        {
            if (GameController.Level == 1) Speed = STANDART_SPEED * 0.75f;
            else if (GameController.Level <= 4) Speed = STANDART_SPEED * 0.85f;
            else Speed = STANDART_SPEED * 0.95f;
        }

        void SpeedToBase() => Speed = SpeedInBlueFear;

        void OffBlueFear()
        {
            if (toBase) return;
            countGhostsInBlueFear--;
            if (countGhostsInBlueFear == 0)
            {
                comboScore = START_COMBO_SCORE;
                GameController.Player.NormalSpeed();
            }
            animation = OldAnimations;
            OldAnimations = null;
            NormalSpeed();
        }

        public override void Move()
        {
            if (toBase)
            {
                Target = Map.Base;
                animation = animationsDied[(int) CurrentDirection];
                if ((AccuratePosition - Map.Base * Map.LENGTH_CELL).Length() < Map.LENGTH_CELL)
                {
                    toBase = false;
                    NormalSpeed();
                    OffBlueFear();
                }
            }
            Position anotherPos = AccuratePosition / Map.LENGTH_CELL;
            desiredDirection =
                GameController.GetDirection(BreadthFirstSearch.FindNextPosition(Map, anotherPos, Target, this) -
                                            anotherPos);
            base.Move();
            if ((Map.PacManWindow.GameController.Player.AccuratePosition - AccuratePosition).Length() <
                Map.LENGTH_CELL)
                if (OldAnimations == null)
                    GameController.PacManWindow.Restart();
                else if (!toBase)
                {
                    toBase = true;
                    startScoreSpawn = DateTime.Now.Ticks;
                    durationScore = 3 * 1000 * 10000;
                    SpeedToBase();
                    score = ComboScore;
                    scoreX = AccuratePosition.x;
                    scoreY = (int) (AccuratePosition.y + 3.333 * Map.LENGTH_CELL);
                    GameController.Score.AddScore(score);
                }
            if (OldAnimations != null)
            {
                long nowTicks = DateTime.Now.Ticks;
                float differenceTimeInSeconds = (nowTicks - startTicksBlueFear) / 10000f / 1000f;
                if (differenceTimeInSeconds > TimeInBlueFear)
                {
                    OffBlueFear();
                    comboScore = START_COMBO_SCORE;
                }
                else if (differenceTimeInSeconds > TimeToReminderEndBlueFear)
                    ToReminderEndBlueFear();
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            if (DateTime.Now.Ticks < startScoreSpawn + durationScore)
                e.Graphics.DrawString(score.ToString(), font, brush, scoreX, scoreY);
        }
    }
}