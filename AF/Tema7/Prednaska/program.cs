// IoC container, Inversion of Control container

using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection()
    .AddSingleton<ILogger, ConsoleLogger>()
    .AddTransient<Banka>();

using var serviceProvider = serviceCollection.BuildServiceProvider();

ILogger logger1 = serviceProvider.GetRequiredService<ILogger>();
logger1.Log("test1");
ILogger logger2 = serviceProvider.GetRequiredService<ILogger>();
logger1.Log("test2");

Banka banka1 = serviceProvider.GetRequiredService<Banka>();
Banka banka2 = serviceProvider.GetRequiredService<Banka>();

banka1.Vyber(10000.0);
banka2.Vyber(10000.0);

interface ILogger
{
    void Log(string message);
}

class ConsoleLogger : ILogger
{
    private Guid id = Guid.NewGuid();

    public void Log(string message)
    {
        Console.WriteLine($"Logger: {id} {DateTime.Now}: {message}");
    }
}

class DebugLogger : ILogger
{
    public void Log(string message)
    {
        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {message}");
    }
}

class Banka
{
    private Guid id = Guid.NewGuid();
    ILogger logger;

    public Banka(ILogger logger)
    {
        this.logger = logger;
    }

    public void Vyber(double castka)
    {
        logger.Log($"Banka id: {id} vyber castky: {castka}");
    }
}
