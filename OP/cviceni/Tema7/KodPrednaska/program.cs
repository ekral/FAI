ILogger logger = new DebugLogger();
BankovniUcet ucet = new BankovniUcet(logger);
ucet.Vloz(2000);
ucet.Vloz(1000);

Nastaveni nastaveni1 = Nastaveni.Instance;
Nastaveni nastaveni2 = Nastaveni.Instance;

Console.WriteLine("Hello, World!");

static class StaticLogger
{

    public static void Log(string message)
    {
        string log = $"{DateTime.Now}: {message}";
        System.IO.File.AppendAllText("log.txt", log + System.Environment.NewLine);
    }
}

public interface ILogger
{
    void Log(string message);
}

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        string log = $"{DateTime.Now}: {message}";
        System.IO.File.AppendAllText("log.txt", log + System.Environment.NewLine);
    }
}

public class DebugLogger : ILogger
{
    public void Log(string message)
    {
        string log = $"{DateTime.Now}: {message}";
        System.Diagnostics.Debug.WriteLine(log);
    }
}

public class SingletonLogger
{
    static SingletonLogger instance;

    private SingletonLogger()
    {

    }
    public void Log(string message)
    {
        string log = $"{DateTime.Now}: {message}";
        System.IO.File.AppendAllText("log.txt", log + System.Environment.NewLine);
    }

    public static SingletonLogger Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = new SingletonLogger();
            }

            return instance;
        }
    }
}

public class BankovniUcet
{
    private ILogger logger;
    public double Zustatek { get; private set; }

    public BankovniUcet(ILogger logger)
    {
        this.logger = logger;
        Zustatek = 0.0;
    }

    public void Vloz(double castka)
    {
        logger.Log($"Vklad na ucet {castka}");
        Zustatek += castka;
    }
}

class Nastaveni
{
    static Nastaveni nastaveni;

    public bool Tisk { get; set; }

    private Nastaveni()
    {

    }

    public static Nastaveni Instance => nastaveni ??= new Nastaveni();

}
