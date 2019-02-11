using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : IControlled
    {
        GameController GameController { get; }
        Bitmap Bitmap { get; }
        public Direction CurrentDirection { get; private set; }
        public Position AccuratePosition { get; private set; }
//        public Position PositionRegardingMapCells => 
//            new Position((AccuratePosition.x + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL,
//                         (AccuratePosition.y + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL);

        public PacMan(GameController gameController, Position accuratePostion)
        {
            //TODO перестать хардкодить
            Bitmap = new Bitmap("../../Pictures/Player.png");
            CurrentDirection = Direction.Right;
            AccuratePosition = accuratePostion;
            GameController = gameController;
            GameController.PacManWindow.KeyDown += Move;
        }

        void Move(object sender, KeyEventArgs e)
        {
            GameController.PacManWindow.Text = e.KeyCode.ToString();
            switch (e.KeyCode)
            {
                case Keys.W:
                    AccuratePosition = GameController.Move(this, Direction.Down);
                    break;
                case Keys.D:
                    AccuratePosition = GameController.Move(this, Direction.Right);
                    break;
                case Keys.S:
                    AccuratePosition = GameController.Move(this, Direction.Up);
                    break;
                case Keys.A:
                    AccuratePosition = GameController.Move(this, Direction.Left);
                    break;
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            GameController.DrawImageAccuratePos(e, Bitmap, AccuratePosition);
        }
    }
}