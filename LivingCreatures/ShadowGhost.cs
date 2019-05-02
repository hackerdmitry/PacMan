using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PacMan
{
    public class ShadowGhost : Ghost
    {
        public ShadowGhost(GameController gameController, Position accuratePostion, Map map) : base(
            gameController, accuratePostion,
            Directory.GetFiles("../../Pictures/ShadowGhost").Select(x => new Bitmap(x)).ToArray(), map) { }

        public override void Move()
        {
            Target = Map.PacManWindow.gameController.creatures[0].AccuratePosition / Map.LENGTH_CELL;
            base.Move();
        }
    }
}