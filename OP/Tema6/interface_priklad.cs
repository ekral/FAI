using System;

namespace Test6
{

    interface IZvuk
    {
        string VratZvuk();
    }

    class Pejsek : IZvuk
    {
        public string VratZvuk()
        {
            return "haf haf";
        }
    }

    class Auto : IZvuk
    {
        public string VratZvuk()
        {
            return "brmmm brmmm";
        }
    }


    class Program
    {
        static void VypisZvuk(IZvuk objektSeZvukem)
        {
            Console.WriteLine(objektSeZvukem.VratZvuk());
        }

        static void Main(string[] args)
        {
            Pejsek pejsek = new Pejsek();
            Auto auto = new Auto();

            VypisZvuk(pejsek);
            VypisZvuk(auto);
        }
    }
}
