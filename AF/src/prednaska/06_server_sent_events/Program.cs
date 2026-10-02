using System.Threading.Channels;

Console.WriteLine("Synchronni foreach a yield");

RunEnum();

Console.WriteLine("Asynchronni foreach a yield");

_ = RunAsyncEnum();

for (int cislo = 0; cislo < 20; cislo++)
{
    await Task.Delay(200);
    Console.WriteLine($"delam neco jineho: {cislo}");
}

Console.WriteLine("Channel");

Channel<int> channel = Channel.CreateBounded<int>(new BoundedChannelOptions(10) { FullMode = BoundedChannelFullMode.DropOldest });

_ = Task.Run(() => RunWriter(channel.Writer));

await foreach (int znak in channel.Reader.ReadAllAsync())
{
    Console.WriteLine($"Zadal jsi znak: {znak}");
}

Console.WriteLine("Konec programu");

static async Task RunAsyncEnum()
{
    await foreach (int cislo in CislaAsync())
    {
        Console.WriteLine(cislo);
    }
}

static void RunEnum()
{
    foreach (int cislo in Cisla())
    {
        Console.WriteLine(cislo);
    }
}

static void RunWriter(ChannelWriter<int> chanelWriter)
{
    ConsoleKeyInfo keyInfo;

    do
    {
        keyInfo = Console.ReadKey(true);

        chanelWriter.TryWrite(keyInfo.KeyChar);

    } while (keyInfo.Key != ConsoleKey.Escape);

    chanelWriter.Complete();
}

static async IAsyncEnumerable<int> CislaAsync()
{
    await Task.Delay(1000);
    yield return 1;
    await Task.Delay(1000);
    yield return 2;
    await Task.Delay(1000);
    yield return 3;
}

static IEnumerable<int> Cisla()
{
    Thread.Sleep(1000);
    yield return 1;
    Thread.Sleep(1000);
    yield return 2;
    Thread.Sleep(1000);
    yield return 3;
}