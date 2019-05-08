using System.Drawing;

namespace PacMan
{
    public interface IDots
    {
        int Value { get; }
        Bitmap Bitmap { get; }
        void Act(GameController gameController);
    }
}