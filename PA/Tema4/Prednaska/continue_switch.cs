//int key;

//while((key = Console.Read()) != 'q')
//{
//    Console.WriteLine($"code: {key} char: {(char)key}");
//}

//for (int i = 0; i < 10; i++)
//{
//    ConsoleKey consoleKey = Console.ReadKey().Key;
//    if (consoleKey == ConsoleKey.Escape) continue;
//    Console.WriteLine(i);
//}


for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        ConsoleKey consoleKey = Console.ReadKey(true).Key;
        if (consoleKey == ConsoleKey.Escape)
        {
            goto konec;
        }

        Console.WriteLine($"i: {i} j: {j} consoleKey {consoleKey}");
    }
}

konec:

static void Metoda()
{
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            ConsoleKey consoleKey = Console.ReadKey(true).Key;

            switch (consoleKey)
            {
                case ConsoleKey.Backspace:

                    break;
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.Clear:
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Pause:
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    break;
            }
            if (consoleKey == ConsoleKey.Escape)
            {
                return;
            }

            Console.WriteLine($"i: {i} j: {j} consoleKey {consoleKey}");
        }
    }
}
