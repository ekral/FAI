using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.CursorVisible = false;

            int width = 20;
            int height = 10;

            int x = 0;
            int y = 0;

            int xAuto = 0;
            int yAuto = height / 2;

            char[,] matice = new char[height, width];

            do
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey klavesa = Console.ReadKey().Key;

                    switch (klavesa)
                    {
                        case ConsoleKey.LeftArrow:
                            if (x > 0)
                            {
                                --x;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (x < (width - 1))
                            {
                                ++x;
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (y > 0)
                            {
                                --y;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (y < (height - 1))
                            {
                                ++y;
                            }
                            break;

                    }
                }

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        matice[i, j] = '-';
                    }
                }

                matice[y, x] = 'z';
                matice[yAuto, xAuto] = 'a';

                Console.SetCursorPosition(0, 0);

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Console.Write(matice[i, j]);
                    }
                    Console.WriteLine();
                }

                ++xAuto;
                if (xAuto > (width - 1))
                {
                    xAuto = 0;
                }
                System.Threading.Thread.Sleep(40);

            } while (true);
        }
    }
}
