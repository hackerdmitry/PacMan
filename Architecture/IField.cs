using System.Drawing;

namespace PacMan
{
    public interface IField
    {
        int X { get; }
        int Y { get; }
        bool IsWall(Creature creature);
        Bitmap Bitmap { get; }
    }
}