#include <iostream>
#include <assert.h>
#include <algorithm>
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

    // kopirovaci konstruktor
    // typ reference
    Stack(const Stack& other) : n(other.n), data(new int[n]), pos(0)
    {
        std::copy_n(other.data, pos, data);
    }

    void operator = (const Stack& other) = delete;
    
    ~Stack()
    {
        delete[] data;
    }

    void push(const int x)
    {
        assert(pos < n);

        data[pos] = x;
        ++pos;
    }

    int pop()
    {
        assert(pos > 0);

        --pos;
        return data[pos];
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

// pouzity typ reference
void print(const Stack& stack)
{
    for(int i = 0; i < stack.count(); ++i)
    {
        std::cout << stack[i] << std::endl;
    }
}
int main()
{
    Stack stack(10);
    stack.push(1);
    stack.push(2);
    stack.push(3);

    Stack copy = stack;
    
    print(stack);

    std::cout << stack.pop() << std::endl;
    std::cout << stack.pop() << std::endl;
    std::cout << stack.pop() << std::endl;

    std::cout << stack.count() << std::endl;
    return 0;
}
