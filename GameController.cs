using System;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public class GameController
    {
        public Map Map { get; }
        public PacManWindow PacManWindow { get; }

//        public static readonly Size DefaultSize = new Size(Map.LENGTH_CELL, Map.LENGTH_CELL);

        //TODO сделать поприличнее
        PacMan player;

        public GameController(PacManWindow pacManWindow, Map map)
        {
            PacManWindow = pacManWindow;
            Map = map;

            //TODO брать позиции существ из файла
            player = new PacMan(this, new Position(1, 1) * Map.LENGTH_CELL);
        }

        public Position Move(IControlled creature, Direction direction)
        {
            Position newAccuratePos = creature.AccuratePosition + GetPosition(direction);
            Position creaturePos = GetPositionRegardingMapCells(newAccuratePos);
            PacManWindow.Text = $"{Map.GetField(creaturePos)} {creaturePos.x} {creaturePos.y}";
            return Map.GetField(creaturePos).IsWall ? creature.AccuratePosition : newAccuratePos;
        }

        public static Position GetPositionRegardingMapCells(Position accuratePosition) =>
            (accuratePosition + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL;

        public static Position GetPosition(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return new Position(0, 1);
                case Direction.Down: return new Position(0, -1);
                case Direction.Left: return new Position(-1, 0);
                case Direction.Right: return new Position(1, 0);
            }
            throw new ArgumentException();
        }

        public static void DrawImageAccuratePos(PaintEventArgs e, Bitmap image, Position accuratePosition) =>
            e.Graphics.DrawImage(
                image,
                new Rectangle(accuratePosition.x, accuratePosition.y,
                              Map.LENGTH_CELL, Map.LENGTH_CELL));

        public static void DrawImageRegardingMapCells(PaintEventArgs e, Bitmap image, Position accuratePosition) =>
            e.Graphics.DrawImage(
                image,
                new Rectangle(accuratePosition.x * Map.LENGTH_CELL, accuratePosition.y * Map.LENGTH_CELL,
                              Map.LENGTH_CELL, Map.LENGTH_CELL));

        public void OnPaint(PaintEventArgs e)
        {
            Map.OnPaint(e);
            player.OnPaint(e);
            e.Graphics.ResetTransform();
        }
    }
}