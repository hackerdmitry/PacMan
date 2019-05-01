using System.Drawing;

namespace PacMan
{
    public interface IField
    {
        bool IsWall { get; }
        Bitmap Bitmap { get; }
    }
}