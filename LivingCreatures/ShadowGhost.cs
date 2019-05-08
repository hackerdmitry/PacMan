using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PacMan
{
    public class ShadowGhost : Ghost
    {
        static readonly Bitmap[][] animations =
        {
            Directory.GetFiles("../../Pictures/ShadowGhost/ShadowDown").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/ShadowGhost/ShadowRight").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/ShadowGhost/ShadowUp").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/ShadowGhost/ShadowLeft").Select(x => new Bitmap(x)).ToArray()
        };

        public ShadowGhost(GameController gameController, Position accuratePostion, Map map) : base(
            gameController, accuratePostion,
            animations[0], map) { }

        public override void Move()
        {
            Target = Map.PacManWindow.GameController.creatures[0].AccuratePosition / Map.LENGTH_CELL;
            base.Move();
            if (OldAnimations == null)
                animation = animations[(int) CurrentDirection];
        }
    }
}