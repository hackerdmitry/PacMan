using System.Drawing;

namespace PacMan
{
    public interface IControlled
    {
        Direction CurrentDirection { get; set; }

        Position AccuratePosition { get; }
        
        float Speed { get; }

        Bitmap Bitmap { get; }
//        Position PositionRegardingMapCells { get; }

        void Move();
    }
}