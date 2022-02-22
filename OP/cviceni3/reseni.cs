using Spectre.Console;
using System;
using System.IO;

namespace ConsoleApp2
{
    public class VypocetHypoteky
    {
        public static double Splatka(double dluh, double urok, int pocetLet)
        {
            double i = urok / 100 / 12;
            int n = pocetLet * 12;

            double v = 1 / (1 + i);
            double splatka = i * dluh / (1 - Math.Pow(v, n));

            return splatka;
        }
    }

    public class Hypoteka
    {
        public double Dluh { get; set; }
        public double Urok { get; set; }
        public int PocetLet { get; set; }

        public Hypoteka(double dluh, double urok, int pocetLet)
        {
            Dluh = dluh;
            Urok = urok;
            PocetLet = pocetLet;
        }

        public double SpocitejSplatku()
        {
            double splatka = VypocetHypoteky.Splatka(Dluh, Urok, PocetLet);
            return splatka;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // S vyuzitim knihovny Spectre.Console, vytvorte uzivatelske rozhrani
            // Uzivatel zada:
            // dluh
            // rocni urok v procentech
            // pocet let splaceni

            // aplikace vypise vysi splatky
            // A zepta se uzivatele zda chce novy vypocet nebo konec aplikace
            bool end = false;

            // todo osetrit vyjimky
            using StreamWriter writer = new StreamWriter("hypoteka.txt", append: true);

            do
            {
                double dluh = AnsiConsole.Ask<double>("Zadej vysi [green]dluhu[/]:");
                Console.WriteLine(dluh);
                int pocetlet = AnsiConsole.Ask<int>("Zadej pocet [red]let[/]");
                Console.WriteLine(pocetlet);
                double urok = AnsiConsole.Ask<double>("Zadej vysi [olive]uroku[/]:");
                Console.WriteLine(urok);

                double splatka = VypocetHypoteky.Splatka(dluh, urok, pocetlet);
                Console.WriteLine(splatka);


                writer.WriteLine($"Date: {DateTime.Now} {System.Environment.NewLine} Dluh: {dluh}{System.Environment.NewLine}Doba: {pocetlet}{System.Environment.NewLine}" +
                    $"urok: {urok}{System.Environment.NewLine}splatka: {splatka}");

                if (!AnsiConsole.Confirm("Pokracovat?"))
                {
                    end = true;
                }
                //Console.Clear();
            } while (!end);
        }
    }
}
