namespace Prednaska3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? retezec = Console.ReadLine();

            if (retezec is not null)
            {
                if (int.TryParse(retezec, out int cislo))
                {
                    Console.WriteLine(cislo);
                }
            }

            int x = 8;
            int y = 11;

            int z = x & y; // bitwise and 1010 & 1011 = 1010
            //  1   1   1
            //  0   0   0
            //  1   1   1
            //  0   0   1

            bool b = x > 0 && y > 0;

            double t1 = 60.0;
            double t2 = 70.0;
            double t3 = 80.0;

            // vypiste nejvyssi pocet bodu dosazeny v testech
            if (t1 > t2 && t1 > t3)
            {
                Console.WriteLine(t1);
            } else if(t2 > t3)
            {
                Console.WriteLine(t2);
            }
            else
            {
                Console.WriteLine(t3);
            }

            double max = t1;
            if (t2 > max) max = t2;
            if (t3 > max) max = t3;


            // bud splnil kazdy z testu t1 t2 za vice nez 50
            // nebo splnil t3 na 100

            if ((t1 > 50 && t2 > 50) || (t3 >= 100))
            {

            }

            // splnil kazdy z testu za vice nez 50 bodu
            if(t1 > 50)
            {
                if(t2 > 50)
                {
                    Console.WriteLine("splnil");
                }
            }

            if(t1 > 50 && t2 > 50)
            {
                Console.WriteLine("splnil");
            }

            // splnil alespon jeden z testu

            if(t1 > 50 || t2 > 50)
            {

            }



            if (x > 0)
            {
                Console.WriteLine("x je vetsi nez 0");
            }
            else if (x == 0)
            {
                Console.WriteLine("x je rovno 0");
            }
            else
            {
                Console.WriteLine("x je mensi nebo rovno 0");
            }
            
        }
    }
}
