// Klientsky kod
static class Test
{
    public static void Print(IMyInterface obj)
    {
        string message = obj.Method();
        TestConsole.WriteLine(message);
    }
}


class Solution 
{
    public static void Run(int n)
    {
        MyClass mc = new MyClass();

        Test.Print(mc); // Vypiše "MyClass.Method" - Print má typ parametru IMyInterface, viz ukázkový kód výše
    }
}


interface IMyInterface
{
    string Method();
}

class MyClass : IMyInterface
{
    public string Method()
    {
        return "MyClass.Method";
    }
}