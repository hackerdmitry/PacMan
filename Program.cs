using System;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PacMan
{
    static class Program
    {
        static void Main()
        {
            Game.CreateMap();
            while (true)
                Application.Run(new PacManWindow("../../Levels/StandartMap"));
        }
    }
}