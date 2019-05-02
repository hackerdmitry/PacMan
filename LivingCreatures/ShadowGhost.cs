using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PacMan
{
    public class ShadowGhost : Creature
    {
        public ShadowGhost(GameController gameController, Position accuratePostion, Bitmap[] animation) : base(
            gameController, accuratePostion, animation)
        {
            //Directory.GetFiles("../../Pictures/Player").Select(x => new Bitmap(x)).ToArray();
        }

        public override float Speed { get; }
        public override Bitmap Bitmap { get; }

        public override void Move()
        {
            
        }
    }
}