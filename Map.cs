using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class Map
    {
        public const int LENGTH_CELL = 32; // кратно DEFAULT_LENGTH_CELL
        public const int DEFAULT_LENGTH_CELL = 8;
        public readonly IField[,] fields;
        public readonly char[,] charFields;
        public int HeightCountCell { get; }
        public int WidthCountCell { get; }
        public Size SizeCountCells { get; }

        readonly Dictionary<char, Func<int, int, IField>> dictionaryFields;

        public Map(char[,] charFields)
        {
            WidthCountCell = charFields.GetLength(1);
            HeightCountCell = charFields.GetLength(0);
            SizeCountCells = new Size(WidthCountCell, HeightCountCell);
            fields = new IField[WidthCountCell, HeightCountCell];
            this.charFields = charFields;
            dictionaryFields = new Dictionary<char, Func<int, int, IField>>
            {
                {'w', (x, y) => new Wall(this, x, y)},
                {' ', (x, y) => new SimpleField(x, y)},
                {'0', (x, y) => new VoidField(x, y)}
            };

            for (int i = 0; i < WidthCountCell; i++)
                for (int j = 0; j < HeightCountCell; j++)
                        fields[i, j] = dictionaryFields[charFields[j, i]](j, i);
        }

        public bool IsCorrectPos(int x, int y) =>
            x >= 0 && y >= 0 && x < fields.GetLength(0) && y < fields.GetLength(1);

        public IField GetField(Position posRegardingMapCells) => fields[posRegardingMapCells.x, posRegardingMapCells.y];

        public void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < WidthCountCell; i++)
                for (int j = 0; j < HeightCountCell; j++)
                    GameController.DrawImageRegardingMapCells(e, fields[i, j].Bitmap, new Position(i, j));
        }
    }
}