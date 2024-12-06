# Šablona třídy Stack

Změńte třídu `Stack` na šablonu tak, aby byl validní kód ve funkci main.

```cpp
#include <iostream>
#include <string>
#include <algorithm>

using namespace std;

// Zmente na template
template<typename T>
class Stack
{
private:
    int n;
    int* data;
    int pos;
public:
    explicit Stack(const int n) : n(n), data(new int[n]), pos(0)
    {
    }

    Stack(const Stack& other) : n(other.n), data(new int[n]), pos(0)
    {
        std::copy_n(other.data, n, data);
    }

    Stack(Stack&&) noexcept = default;
    Stack& operator=(Stack&& other) noexcept = default;

    Stack& operator=(Stack other)
    {
        swap(n, other.n);
        swap(data, other.data);
        swap(pos, other.pos);

        return *this;
    }

    ~Stack()
    {
        delete[] data;
    }

    void push(const int x)
    {
        data[pos] = x;
        ++pos;
    }

    int top()
    {
        return data[pos - 1];
    }
    
    void pop()
    {
        --pos;

    }

    int operator[] (const int i) const
    {
        return data[i];
    }

    int count() const
    {
        return pos;
    }
};

int main()
{
    Stack<int> stackCisla(10);

    stackCisla.push(1);
    stackCisla.push(2);
    stackCisla.push(3);

    for (int i = 0; i < stackCisla.count(); i++)
    {
        cout << stackCisla[i] << endl;
    }

    cout << stackCisla.top() << endl;
    stackCisla.pop();

    cout << stackCisla.top() << endl;
    stackCisla.pop();

    cout << stackCisla.top() << endl;
    stackCisla.pop();

    Stack<string> stackJmena(10);

    stackJmena.push("Jiri");
    stackJmena.push("Karel");
    stackJmena.push("Alena");

    cout << stackJmena.top() << endl;
    cout << stackJmena[0] << endl;
    cout << stackJmena[1] << endl;
    cout << stackJmena[2] << endl;

    return 0;
}
```
