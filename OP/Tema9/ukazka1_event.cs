using System;

namespace Tema9
{
    class Hlidac
    {
        public event Action poplach;

        public void Loupez()
        {
            poplach?.Invoke();
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            Hlidac hlidac = new Hlidac();
            //hlidac.poplach = () => Console.WriteLine("volam policiu 158");
            //hlidac.poplach.Invoke();

            hlidac.poplach += () => Console.WriteLine("volam policiu 158");

            hlidac.Loupez();
        }
    }
}
