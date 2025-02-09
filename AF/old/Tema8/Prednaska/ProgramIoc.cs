using Microsoft.Extensions.DependencyInjection;

//Sluzba sluzba = new Sluzba();
//Trida trida1 = new Trida(sluzba);
//Trida trida2 = new Trida(sluzba);

IServiceCollection serviceCollection = new ServiceCollection()
    .AddSingleton<ISluzba, SluzbaB>()
    .AddTransient<Trida>();

using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

Trida trida1 = serviceProvider.GetRequiredService<Trida>();
Trida trida2 = serviceProvider.GetRequiredService<Trida>();

trida1.Test();
trida2.Test();

interface ISluzba
{
    void Metoda();
}

class SluzbaA : ISluzba
{
    public void Metoda()
    {
        Console.WriteLine("Sluzba A");
    }
}

class SluzbaB : ISluzba
{
    public void Metoda()
    {
        Console.WriteLine("Sluzba B");
    }
}
class Trida
{
    private ISluzba sluzba;

    public Trida(ISluzba sluzba)
    {
        this.sluzba = sluzba;
    }

    public void Test()
    {
        sluzba.Metoda();
    }
}
