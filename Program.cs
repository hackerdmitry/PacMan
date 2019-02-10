using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Game.CreateMap();
            Application.Run(new PacManWindow());
        }
    }
}