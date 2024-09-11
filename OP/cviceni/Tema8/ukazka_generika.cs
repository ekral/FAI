using System;
using System.Collections.Generic;

namespace Tema8
{
    class MojeTrida<T>
    {
        public T x;
    }
    
    abstract class Zviratko
    {
        public string Jmeno { get; set; }
        public abstract string Zvuk();
    }

    class Pejsek : Zviratko
    {
        public override string Zvuk()
        {
            return "haf haf";
        }
    }

    class Kocicka : Zviratko
    {
        public override string Zvuk()
        {
            return "mnau";
        }
    }

    class MojeDalsiTrida<T> where T : Zviratko
    {
        public T y;

        public MojeDalsiTrida()
        {
            y = default(T);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MojeDalsiTrida<Pejsek> mojeDalsiTrida1 = new MojeDalsiTrida<Pejsek>();
            MojeDalsiTrida<Kocicka> mojeDalsiTrida2 = new MojeDalsiTrida<Kocicka>();

            MojeTrida<int> mojeTrida1 = new MojeTrida<int>(); 
            MojeTrida<double> mojeTrida2 = new MojeTrida<double>(); 
        }
    }
}
