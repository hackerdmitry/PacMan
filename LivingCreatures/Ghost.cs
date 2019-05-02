using System.Drawing;
using System.IO;
using System.Linq;

namespace PacMan
{
    public class Ghost : Creature
    {
        protected readonly Map Map;
        protected Position Target;

        protected Ghost(GameController gameController, Position accuratePostion, Bitmap[] animation, Map map) : base(
            gameController, accuratePostion, animation)
        {
            Map = map;
            Target = new Position(0, 5);
        }

        public override float Speed => 1f;


        public override void Move()
        {
            Position anotherPos = AccuratePosition / Map.LENGTH_CELL;
            desiredDirection =
                GameController.GetDirection(BreadthFirstSearch.FindNextPosition(Map, anotherPos, Target) - anotherPos);
            base.Move();
        }
    }
}