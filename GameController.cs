using System;
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

        public GameController(Map map)
        {
            Map = map;
        }

        public Position Move(IControlled creature, Direction direction)
        {
            Position newAccuratePos = creature.AccuratePosition + GetPosition(direction);
            Position creaturePos = GetPositionRegardingMapCells(newAccuratePos);
            return Map.GetField(creaturePos).IsWall ? creature.AccuratePosition : newAccuratePos;
        }
        
        public Position GetPositionRegardingMapCells(Position accuratePosition) => 
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

        public void OnPaint(PaintEventArgs e)
        {
            Map.OnPaint(e);
        }
    }
}