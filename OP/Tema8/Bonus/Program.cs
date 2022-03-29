Test<int> test = new Test<int>();
test.x = 1;
test.y = 2;

int soucet = test.Soucet();

class Test<T> where T : struct, INumber<T>
{
    public T x;
    public T y;

    public T Soucet()
    {
        return x + y;
    }
}