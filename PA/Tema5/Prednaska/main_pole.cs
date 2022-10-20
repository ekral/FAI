int x = 2;
int y = 3;
int[] pole = null;
pole = new int[3]; // tri prvky
pole[0] = 1; // prvni
pole[1] = 2; // druhy
pole[2] = 3; // treti

if (true)
{
    int[] kopie = pole;
    kopie[0] = 7;
    kopie[1] = 7;
    kopie[2] = 7;
}

Console.WriteLine(String.Join(",", pole));
// jake vypise prvky?
Console.WriteLine(String.Join(",", kopie));
// jake vypise prvky?

Console.WriteLine(pole.Length);
int nejvyssiIndex = pole.Length - 1;

Console.WriteLine("konec");
