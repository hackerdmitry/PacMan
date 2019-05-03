using System.Drawing;

namespace PacMan
{
    public class Ghost : Creature
    {
        public override float Speed => 2.5f;
        
        protected readonly Map Map;
        protected Position Target;

        protected Ghost(GameController gameController, Position accuratePostion, Bitmap[] animation, Map map) : base(
            gameController, accuratePostion, animation, 100)
        {
            Map = map;
            Target = new Position(0, 5);
        }


        public override void Move()
        {
            Position anotherPos = AccuratePosition / Map.LENGTH_CELL;
            desiredDirection =
                GameController.GetDirection(BreadthFirstSearch.FindNextPosition(Map, anotherPos, Target) - anotherPos);
            base.Move();
        }
    }
}