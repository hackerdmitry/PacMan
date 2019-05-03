using System.Collections.Generic;
using System.Drawing;

namespace PacMan
{
    public class SpawnWall : IField
    {
        public SpawnWall(Map map, int x, int y)
        {
            X = x;
            Y = y;
            Bitmap = new Bitmap("../../Pictures/SpawnWalls/WallTo" +
                                $"{GetIntCell(map, X, Y - 1)}" +
                                $"{GetIntCell(map, X - 1, Y)}" +
                                $"{GetIntCell(map, X, Y + 1)}" +
                                $"{GetIntCell(map, X + 1, Y)}.png");
        }

        int GetIntCell(Map map, int x, int y) =>
            map.IsCorrectPos(y, x) && map.MapFields.CharFields[x, y] == 's' ? 1 : 0;

        public int X { get; }
        public int Y { get; }
        public bool IsWall => true;
        public Bitmap Bitmap { get; }
    }
}