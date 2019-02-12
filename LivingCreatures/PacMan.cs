using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : IControlled
    {
        GameController GameController { get; }
        public Bitmap Bitmap { get; }
        public Direction CurrentDirection { get; set; }
        public Position AccuratePosition { get; private set; }
//        public Position PositionRegardingMapCells => 
//            new Position((AccuratePosition.x + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL,
//                         (AccuratePosition.y + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL);

        Direction desiredDirection;
        public PacMan(GameController gameController, Position accuratePostion)
        {
            //TODO перестать хардкодить
            Bitmap = new Bitmap("../../Pictures/Player.png");
            CurrentDirection = Direction.Right;
            AccuratePosition = accuratePostion;
            GameController = gameController;
            GameController.PacManWindow.KeyDown += ChangeDirection;
        }

        public void Move()
        {
            AccuratePosition = GameController.Move(this, desiredDirection);
            GameController.PacManWindow.Text = $"PacMan {AccuratePosition.x} {AccuratePosition.y}";
        }

        void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    desiredDirection = Direction.Down;
                    break;
                case Keys.D:
                    desiredDirection = Direction.Right;
                    break;
                case Keys.S:
                    desiredDirection = Direction.Up;
                    break;
                case Keys.A:
                    desiredDirection = Direction.Left;
                    break;
            }
        }
    }
}