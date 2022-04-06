// Ukol 4 Observer: // Doplnte spravne klicove slovo, aby nasledujici radky nesly prelozit
                   // a slo o navrhovy vzor observer

using System;

namespace Tema9
{
    class NakupniCentrum
    {
        public string Nazev { get; set; }
        public event Action<string> poplach;

        public NakupniCentrum(string nazev)
        {
            Nazev = nazev;
        }

        public void Pozar()
        {
            poplach?.Invoke(Nazev);
        }
    }
    class Program
    {
        public static void VyjezdHasicu(string nazev)
        {
            Console.WriteLine($"Jedeme hasit {nazev}");
        }

        public static void VyjezdPolicie(string nazev)
        {
            Console.WriteLine($"Jedeme delat pomahat a chranit do {nazev}");
        }

        static void Main(string[] args)
        {
            NakupniCentrum nakupniCentrum = new NakupniCentrum("Zlate Jablko");
            nakupniCentrum.poplach += VyjezdHasicu;
            nakupniCentrum.poplach += VyjezdPolicie;
            nakupniCentrum.poplach -= VyjezdPolicie;

            // Doplnte spravne klicove slovo, aby nasledujici radky nesly prelozit
            // a slo o navrhovy vzor observer
          
            //nakupniCentrum.poplach = VyjezdHasicu;
            //nakupniCentrum.poplach = null;
            //nakupniCentrum.poplach.Invoke("neco");

            nakupniCentrum.Pozar();
        }
    }
}
