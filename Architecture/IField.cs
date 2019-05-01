using System.Drawing;

namespace PacMan
{
    public interface IField
    {
        int X { get; }
        int Y { get; }
        bool IsWall { get; }
        Bitmap Bitmap { get; }
    }
}