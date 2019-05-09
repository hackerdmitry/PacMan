using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PacMan
{
    public class Map
    {
        public const int LENGTH_CELL = 24; // кратно DEFAULT_LENGTH_CELL
        public const int DEFAULT_LENGTH_CELL = 4;
        public readonly MapDots MapDots;
        public MapFields MapFields { get; private set; }
        public int HeightCountCell { get; }
        public PacManWindow PacManWindow { get; }
        public int WidthCountCell { get; }
        public Size SizeCountCells { get; }
        readonly List<Position> bases = new List<Position>();
        readonly Random rand = new Random();
        public Position Base => bases[rand.Next(0, bases.Count)];
        readonly List<FruitField> fruitFields = new List<FruitField>();

        public Map(char[,] charFields, char[,] charDots, PacManWindow pacManWindow)
        {
            PacManWindow = pacManWindow;
            WidthCountCell = charFields.GetLength(1);
            HeightCountCell = charFields.GetLength(0);
            SizeCountCells = new Size(WidthCountCell, HeightCountCell);
            InitMapFields(charFields);
            foreach (IField mapField in MapFields)
            {
                if (mapField is SpawnField)
                    bases.Add(new Position(mapField.Y, mapField.X));
                if (mapField is FruitField field)
                    fruitFields.Add(field);
            }
            MapDots = new MapDots(charDots, this);
        }

        void InitMapFields(char[,] charFields)
        {
            MapFields = new MapFields(charFields, this);
            MapFields.MakeFields();
        }

        public bool IsCorrectPos(int x, int y) =>
            x >= 0 && y >= 0 && x < HeightCountCell && y < WidthCountCell;

        public IField GetField(Position posRegardingMapCells) =>
            MapFields[posRegardingMapCells.x, posRegardingMapCells.y];

        public Position GetPositionInMap(Position position) => (position + SizeCountCells) % SizeCountCells;

        public void InformFruits(int score) => fruitFields.ForEach(x => x.Inform(score, PacManWindow.GameController));

        public void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < WidthCountCell; i++)
                for (int j = 0; j < HeightCountCell; j++)
                    PacManWindow.GameController.DrawImageRegardingMapCells(
                        e, MapFields[i, j].Bitmap, new Position(i, j));
            foreach (KeyValuePair<Position, IDots> mapDot in MapDots)
                PacManWindow.GameController.DrawImageAccuratePos(e, mapDot.Value.Bitmap, mapDot.Key);
            fruitFields.ForEach(x => x.OnPaint(e, PacManWindow.GameController));
        }
    }
}