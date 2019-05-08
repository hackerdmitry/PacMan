using System.Drawing;

namespace PacMan
{
    public class Wall : IField
    {
        public Wall(Map map, int x, int y)
        {
            X = x;
            Y = y;
            Bitmap = new Bitmap("../../Pictures/Walls/WallTo" +
                                $"{GetIntCell(map, X, Y - 1)}" +
                                $"{GetIntCell(map, X - 1, Y)}" +
                                $"{GetIntCell(map, X, Y + 1)}" +
                                $"{GetIntCell(map, X + 1, Y)}.png");
        }

        int GetIntCell(Map map, int x, int y) =>
            map.IsCorrectPos(x, y) && map.MapFields.CharFields[x, y] == 'w' ? 1 : 0;

        public int X { get; }
        public int Y { get; }
        public bool IsWall(Creature creature) => true;
        public Bitmap Bitmap { get; }
    }
}