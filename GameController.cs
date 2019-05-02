using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public readonly List<Creature> creatures;

        public Size SizeMap { get; }

        public GameController(PacManWindow pacManWindow, Map map, Timer timer)
        {
            PacManWindow = pacManWindow;
            Map = map;
            SizeMap = new Size(Map.WidthCountCell * Map.LENGTH_CELL, Map.HeightCountCell * Map.LENGTH_CELL);
            timer.Tick += TimerTick;
            //TODO брать позиции существ из файла
            creatures = new List<Creature>
            {
                new PacMan(this, new Position(1, 1) * Map.LENGTH_CELL),
                new ShadowGhost(this, new Position(21, 5) * Map.LENGTH_CELL, map)
            };
        }

        void TimerTick(object sender, EventArgs args)
        {
            foreach (Creature creature in creatures)
                creature.Move();
        }

        public Position Move(Creature creature, Direction direction)
        {
            Position newAccuratePosByDesiredDir =
                (creature.AccuratePosition + GetPosition(direction) * creature.Speed + SizeMap) % SizeMap;
            if (GetCellsRegardingEdges(newAccuratePosByDesiredDir)
                    .Any(x => Map.GetField((x + Map.SizeCountCells) % Map.SizeCountCells).IsWall) ||
                newAccuratePosByDesiredDir.x % Map.LENGTH_CELL != 0 &&
                newAccuratePosByDesiredDir.y % Map.LENGTH_CELL != 0)
            {
                Position newAccuratePosByCurrentDir =
                    (creature.AccuratePosition + GetPosition(creature.CurrentDirection) * creature.Speed + SizeMap) %
                    SizeMap;
                return GetCellsRegardingEdges(newAccuratePosByCurrentDir)
                    .Any(x => Map.GetField((x + Map.SizeCountCells) % Map.SizeCountCells).IsWall)
                    ? creature.AccuratePosition.Normalize()
                    : newAccuratePosByCurrentDir;
            }
            creature.CurrentDirection = direction;
            return newAccuratePosByDesiredDir;
        }

        public static Position GetPositionRegardingMapCells(Position accuratePosition) =>
            (accuratePosition + Map.LENGTH_CELL / 2) / Map.LENGTH_CELL;

        public static Position[] GetCellsRegardingEdges(Position accuratePosition) => new[]
        {
            accuratePosition / Map.LENGTH_CELL,
            (accuratePosition + (Map.LENGTH_CELL - 1)) / Map.LENGTH_CELL,
            (accuratePosition + new Position(Map.LENGTH_CELL - 1, 0)) / Map.LENGTH_CELL,
            (accuratePosition + new Position(0, Map.LENGTH_CELL - 1)) / Map.LENGTH_CELL
        };

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

        public static Direction GetDirection(Position position)
        {
            if (position.x != 0)
                return position.x < 0 ? Direction.Left : Direction.Right;
            return position.y < 0 ? Direction.Down : Direction.Up;
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
            foreach (Creature creature in creatures)
            {
                Position rightBottomEdge = creature.AccuratePosition + (Map.LENGTH_CELL - 1);
                DrawImageAccuratePos(e, creature.Bitmap, creature.AccuratePosition);
                int x = creature.AccuratePosition.x < 0
                        ? SizeMap.Width
                        : rightBottomEdge.x > SizeMap.Width
                            ? -SizeMap.Width
                            : 0,
                    y = creature.AccuratePosition.y < 0
                        ? SizeMap.Height
                        : rightBottomEdge.y > SizeMap.Height
                            ? -SizeMap.Height
                            : 0;
                DrawImageAccuratePos(e, creature.Bitmap, new Position(creature.AccuratePosition.x + x,
                                                                      creature.AccuratePosition.y + y));
            }
            e.Graphics.ResetTransform();
        }
    }
}