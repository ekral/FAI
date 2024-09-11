using System;

namespace ConsoleApp30
{
    class Program
    {
        static void Metoda1()
        {
            Console.WriteLine("Ahoj");
        }

        static void Metoda2(int x)
        {
            Console.WriteLine(x);
        }

        static int Metoda3()
        {
            return 1;
        }

        static int Metoda4(string x)
        {
            return 1;
        }

        static int Metoda5(string x, double y)
        {
            return 1;
        }

        static bool Metoda6()
        {
            return true;
        }

        static bool Metoda7(int x)
        {
            return x > 0;
        }

        static bool Metoda8(int x, double y)
        {
            return x > y;
        }

        // Nadefinujte MujTyp1 az MujTyp8 jako typ delegat, kod metody Main nemente
        delegate void MujTyp1();
        delegate void MujTyp2(int x);
        delegate int MujTyp3();
        delegate int MujTyp4(string x);
        delegate int MujTyp5(string x, double y);
        delegate bool MujTyp6();
        delegate bool MujTyp7(int x);
        delegate bool MujTyp8(int x, double y);

        static void Main(string[] args)
        {
            MujTyp1 d1 = Metoda1;
            MujTyp2 d2 = Metoda2;
            MujTyp3 d3 = Metoda3;
            MujTyp4 d4 = Metoda4;
            MujTyp5 d5 = Metoda5;
            MujTyp6 d6 = Metoda6;
            MujTyp7 d7 = Metoda7;
            MujTyp8 d8 = Metoda8;
        }
    }
}
