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
        public int HeightCountCell { get; }
        public int WidthCountCell { get; }
        public Size SizeCountCells { get; }

        readonly Dictionary<char, Func<IField>> dictionaryFields = new Dictionary<char, Func<IField>>
        {
            {'w', () => new Wall((int) Walls.SimpleWall)},
            {'u', () => new Wall((int) Walls.WallToUp)},
            {'r', () => new Wall((int) Walls.WallToRight)},
            {'l', () => new Wall((int) Walls.WallToLeft)},
            {'d', () => new Wall((int) Walls.WallToDown)},
            {' ', () => new SimpleField()},
            {'0', () => new VoidField()}
        };

        public Map(char[,] charFields)
        {
            WidthCountCell = charFields.GetLength(1);
            HeightCountCell = charFields.GetLength(0);
            SizeCountCells = new Size(WidthCountCell, HeightCountCell);
            fields = new IField[WidthCountCell, HeightCountCell];

            for (int i = 0; i < WidthCountCell; i++)
                for (int j = 0; j < HeightCountCell; j++)
                    fields[i, j] = dictionaryFields[charFields[j, i]]();
        }

        public IField GetField(Position posRegardingMapCells) => fields[posRegardingMapCells.x, posRegardingMapCells.y];

        public void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < WidthCountCell; i++)
                for (int j = 0; j < HeightCountCell; j++)
                    GameController.DrawImageRegardingMapCells(e, fields[i, j].Bitmap, new Position(i, j));
//                    e.Graphics.DrawImage(fields[i, j].Bitmap, new Rectangle(j * LENGTH_CELL, i * LENGTH_CELL,
//                                             LENGTH_CELL, LENGTH_CELL));
        }
    }
}