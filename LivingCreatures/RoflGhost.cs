using System.Drawing;
using System.IO;
using System.Linq;

namespace PacMan
{
    public class RoflGhost : Ghost
    {
        static readonly Bitmap[][] animations =
        {
            Directory.GetFiles("../../Pictures/RoflGhost/RoflDown").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/RoflGhost/RoflRight").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/RoflGhost/RoflUp").Select(x => new Bitmap(x)).ToArray(),
            Directory.GetFiles("../../Pictures/RoflGhost/RoflLeft").Select(x => new Bitmap(x)).ToArray()
        };

        public RoflGhost(GameController gameController, Position accuratePostion, Map map) : base(
            gameController, accuratePostion,
            animations[0], map) =>
            Target = way[iWay++];

        public override Bitmap Bitmap => animation[iAnimation];

        public override float Speed => 5f;

        readonly Position[] way =
        {
            new Position(1, 1),
            new Position(21, 20),
            new Position(1, 20),
            new Position(21, 1)
        };
        int iWay;

        public override void Move()
        {
            if ((AccuratePosition - Target * Map.LENGTH_CELL).Length() < Speed * 2)
            {
                Target = way[iWay++];
                iWay %= way.Length;
            }
            base.Move();
            animation = animations[(int) CurrentDirection];
        }
    }
}