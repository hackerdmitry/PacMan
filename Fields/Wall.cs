using System.Collections.Generic;
using System.Drawing;

namespace PacMan
{
    enum Walls
    {
        SimpleWall,
        WallToUp,
        WallToRight,
        WallToLeft,
        WallToDown
    }
    
    public class Wall : IField
    {
        readonly Dictionary<Walls, Bitmap> walls = new Dictionary<Walls, Bitmap>
        {
            {Walls.SimpleWall, new Bitmap("../../Pictures/Walls/SimpleWall.png")},
            {Walls.WallToUp, new Bitmap("../../Pictures/Walls/WallToUp.png")},
            {Walls.WallToRight, new Bitmap("../../Pictures/Walls/WallToRight.png")},
            {Walls.WallToLeft, new Bitmap("../../Pictures/Walls/WallToLeft.png")},
            {Walls.WallToDown, new Bitmap("../../Pictures/Walls/WallToDown.png")}
        };

        public Wall(int numWall) => Bitmap = walls[(Walls)numWall];

        public bool IsWall => true;
        public Bitmap Bitmap { get; }
    }
}