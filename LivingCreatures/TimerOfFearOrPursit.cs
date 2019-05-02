using System;
using System.Threading;

namespace PacMan
{
    public class TimerOfFearOrPursit
    {
        int endTimersss;
        int endFear, endPursit;
        Timer timer, timer2, timersss, timer1, timer3, timer4, timer5;

        public TimerOfFearOrPursit()
        {
            timersss = new Timer(Timersss, 0, 0, 27000);
            Console.ReadLine();
        }

        void Timersss(object obj)
        {
            int num = 0;
            timer = new Timer(Fear, num, 0, 7000);
            timer1 = new Timer(Pursuit, num, 7000, 20000);
            if (++endTimersss > 1)
                timersss.Dispose();

            timer2 = new Timer(Fear, num, 54000, 5000);
            timer3 = new Timer(Pursuit, num, 59000, 20000);

            timer4 = new Timer(Fear, num, 79000, 5000);
            timer5 = new Timer(Pursuit, num, 84000, int.MaxValue);
        }

        void Fear(object obj)
        {
            //TODO сделать и запихать сюды методы разбега по углам
            switch (++endFear)
            {
                case 1:
                    timer.Dispose();
                    break;
                case 2:
                    timer2.Dispose();
                    break;
                case 3:
                    timer4.Dispose();
                    break;
            }
        }

        void Pursuit(object obj)
        {
            //TODO сделать и запихать сюды методы преследования
            switch (++endPursit)
            {
                case 1:
                    timer1.Dispose();
                    break;
                case 2:
                    timer3.Dispose();
                    break;
                case 3:
                    timer5.Dispose();
                    break;
            }
        }
    }
}