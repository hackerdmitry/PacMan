using System.Windows.Forms;

namespace PacMan
{
    static class Program
    {
        static void Main()
        {
            Game.CreateMap();
            Application.Run(new PacManWindow());
        }
    }
}