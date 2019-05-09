using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PacMan
{
    public class Ghost : Creature
    {
        public override float Speed { get; protected set; } = 2.5f;
        public static float TimeInBlueFear => 7f;
        public static float SpeedInBlueFear => 7f;
        public static float TimeToReminderEndBlueFear => 5f;

        protected readonly Map Map;
        protected Position Target;

        bool toBase;

        static long startTicksBlueFear;
        float oldSpeed;
        protected Bitmap[] OldAnimations;

        const int START_COMBO_SCORE = 100;
        static int comboScore = START_COMBO_SCORE;
        static int countGhostsInBlueFear;
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
            startTicksBlueFear = DateTime.Now.Ticks;
            OldAnimations = animation;
            animation = blueFearAnimations;
            oldSpeed = Speed;
            Speed /= 2;
        }

        void ToReminderEndBlueFear()
        {
            if (toBase) return;
            animation = reminderEndBlueFearAnimations;
        }

        void OffBlueFear()
        {
            if (toBase) return;
            countGhostsInBlueFear--;
            if (countGhostsInBlueFear == 0)
                comboScore = START_COMBO_SCORE;
            animation = OldAnimations;
            OldAnimations = null;
            Speed = oldSpeed;
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
                    Speed = oldSpeed;
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
                    Speed = SpeedInBlueFear;
                    GameController.Score.AddScore(ComboScore);
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
    }
}