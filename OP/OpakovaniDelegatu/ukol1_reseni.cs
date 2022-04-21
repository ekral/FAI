using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp31
{

    class Program
    {
        private static object locker = new object();
        private static Timer timer;
        static void Vypis(double position, List<string> studenti, ConsoleColor pointerColor, ConsoleColor studentColor)
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < studenti.Count; i++)
            {
                if (i == Math.Floor(position))
                {
                    Console.ForegroundColor = pointerColor;
                    Console.Write(" => ");
                    Console.ForegroundColor = studentColor;
                    Console.WriteLine(studenti[i] + "        ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("    " + studenti[i] + "        ");
                }
            }
        }

        static void Main(string[] args)
        {

            Random rnd = new Random();

            List<string> studenti = new List<string>
            {
                "Kalandrova",
                "Krivogonov",
                "Macalik",
                "Novotny",
                "Popovsky",
                "Pozdnyshev",
                "Prochazka",
                "Sedlacek",
                "Uchytil"
            };

            Console.CursorVisible = false;
            bool start = true;
            double speed = 1.0 + rnd.NextDouble();
            double position = 0;
            int timeInterval = 25;


            timer = new Timer((state) =>
            {
                bool hasLock = false;

                try
                {
                    Monitor.TryEnter(locker, ref hasLock);

                    if (!hasLock)
                    {
                        return;
                    }

                    timer.Change(Timeout.Infinite, Timeout.Infinite);

                    if (start)
                    {
                        start = false;
                        speed = 1.0 + rnd.NextDouble();
                    }

                    if (speed > 0.02)
                    {
                        Vypis(position, studenti, ConsoleColor.Yellow, ConsoleColor.White);

                        position = (position + speed) % studenti.Count;

                        speed *= 0.99;
                    }
                    else
                    {
                        Vypis(position, studenti, ConsoleColor.Yellow, ConsoleColor.Red);
                    }
                }
                finally
                {
                    if (hasLock)
                    {
                        Monitor.Exit(locker);
                        timer.Change(timeInterval, timeInterval);
                    }

                }

            }, null, timeInterval, timeInterval);

            bool konec = false;
            do
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {

                    case ConsoleKey.Escape:
                        konec = true;
                        break;
                    case ConsoleKey.Spacebar:
                        start = true;
                        break;
                    default:
                        break;
                }

            } while (!konec);

            timer.Dispose();
        }
    }
}
