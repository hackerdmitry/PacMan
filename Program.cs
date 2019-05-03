﻿using System.Windows.Forms;

namespace PacMan
{
    static class Program
    {
        public static bool restart = true;
        
        static void Main()
        {
            Game.CreateMap();
            while (restart)
            {
                restart = false;
                Application.Run(new PacManWindow("../../Levels/StandartMap"));
            }
        }
    }
}