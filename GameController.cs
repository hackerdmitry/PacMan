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
        public Map Map { get; private set; }
        public Score Score { get; }
        public Footer Footer { get; }
        public PacManWindow PacManWindow { get; }
        public int Level { get; private set; }

        List<Creature> creatures;

        public PacMan Player;

        public Size SizeMap { get; }
        readonly Timer timer;
        bool isDisposed;

        public GameController(PacManWindow pacManWindow, Timer timer)
        {
            PacManWindow = pacManWindow;
            Level = 0;
            NextLevel();
            Score = new Score(this);
            Footer = new Footer(this);
            this.timer = timer;
            SizeMap = new Size(Map.WidthCountCell * Map.LENGTH_CELL,
                               Map.HeightCountCell * Map.LENGTH_CELL);
            timer.Tick += TimerTick;
            CreateCreatures(Map);
        }

        void CreateCreatures(Map map)
        {
            string[] notParsedCreatures = new StreamReader($"{map.PacManWindow.FolderLevelPath}/creatures.txt")
                .ReadToEnd()
                .Split('\n', '\r').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Dictionary<string, Func<int, int, Creature>> dictionaryCreatures =
                new Dictionary<string, Func<int, int, Creature>>
                {
                    {"p", (x, y) => new PacMan(this, new Position(x, y) * Map.LENGTH_CELL)},
                    {"s", (x, y) => new ShadowGhost(this, new Position(x, y) * Map.LENGTH_CELL, map)},
                    {"r", (x, y) => new RoflGhost(this, new Position(x, y) * Map.LENGTH_CELL, map)}
                };
            creatures = new List<Creature>();
            foreach (string creature in notParsedCreatures)
            {
                string[] data = creature.Split();
                creatures.Add(dictionaryCreatures[data[0]]
                              ((int) float.Parse(data[1]),
                               (int) float.Parse(data[2])));
            }
            Player = (PacMan) (creatures.First(x => x is PacMan) ??
                               new PacMan(this, new Position(0, 0) * Map.LENGTH_CELL));
        }

        void TimerTick(object sender, EventArgs args)
        {
            foreach (Creature creature in creatures)
                creature.Move();
        }

        public void NextLevel()
        {
            Map = PacManWindow.ClearMap();
            Level++;
        }

        public Position Move(Creature creature, Direction direction)
        {
            Position newAccuratePosByDesiredDir =
                (creature.AccuratePosition + GetPosition(direction) * creature.Speed + SizeMap) % SizeMap;

            Position differencePos = newAccuratePosByDesiredDir - creature.AccuratePosition.Normalize();
            int absPos = Math.Abs(differencePos.x) + Math.Abs(differencePos.y);
            if (absPos < creature.Speed * 2 &&
                direction != creature.CurrentDirection &&
                !Map.GetField(Map.GetPositionInMap(newAccuratePosByDesiredDir.Normalize() / Map.LENGTH_CELL +
                                                   GetPosition(direction))).IsWall(creature))
            {
                creature.CurrentDirection = direction;
                return creature.AccuratePosition.Normalize() + GetPosition(direction) * absPos;
            }

            if (GetCellsRegardingEdges(newAccuratePosByDesiredDir)
                    .Any(x => Map.GetField((x + Map.SizeCountCells) % Map.SizeCountCells).IsWall(creature)) ||
                newAccuratePosByDesiredDir.x % Map.LENGTH_CELL != 0 &&
                newAccuratePosByDesiredDir.y % Map.LENGTH_CELL != 0)
            {
                Position newAccuratePosByCurrentDir =
                    (creature.AccuratePosition + GetPosition(creature.CurrentDirection) * creature.Speed + SizeMap) %
                    SizeMap;
                return GetCellsRegardingEdges(newAccuratePosByCurrentDir)
                    .Any(x => Map.GetField((x + Map.SizeCountCells) % Map.SizeCountCells).IsWall(creature))
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

        public void DrawImageAccuratePos(PaintEventArgs e, Bitmap image, Position accuratePosition) =>
            e.Graphics.DrawImage(
                image,
                new Rectangle(accuratePosition.x, Score.Height + accuratePosition.y,
                              Map.LENGTH_CELL, Map.LENGTH_CELL));

        public void DrawImageRegardingMapCells(PaintEventArgs e, Bitmap image, Position accuratePosition) =>
            DrawImageAccuratePos(e, image, accuratePosition * Map.LENGTH_CELL);

        public void ToBlueFearGhosts() => creatures.ForEach(x =>
        {
            if (x is Ghost ghost) ghost.ToBlueFear();
        });

        public void OnPaint(PaintEventArgs e)
        {
            Map.OnPaint(e);
            Score.OnPaint(e);
            Footer.OnPaint(e);
            foreach (Creature creature in creatures)
                if (creature is Ghost ghost)
                    ghost.OnPaint(e);
            foreach (Creature creature in creatures)
            {
                if (!creature.timer.Enabled) continue;
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

        public void DisposeWithDiePlayer()
        {
            timer.Tick -= TimerTick;
            creatures.ForEach(x => x.timer.Stop());
            Player.timer.Start();
            Player.Die();
            isDisposed = true;
        }

        public void Dispose()
        {
            timer.Tick -= TimerTick;
            creatures.ForEach(x => x.timer.Stop());
            isDisposed = true;
        }

        public void Restart()
        {
            if (!isDisposed) return;
            Player.timer.Stop();
            timer.Tick += TimerTick;
            CreateCreatures(Map);
            isDisposed = false;
        }

        public void StopTimer()
        {
            timer.Stop();
            Program.restart = true;
        }
    }
}