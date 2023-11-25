#include <iostream>
#include <vector>

using namespace std;

class Zviratko
{
public:
    // pure virtual funkction
    virtual void Zvuk() = 0;
};

class Kocicka : public Zviratko
{
    void Zvuk() override
    {
        cout << "mnau" << endl;
    }
};

class Pejsek : public Zviratko
{
    void Zvuk() override
    {
        cout << "haf haf" << endl;
    }
};

template<class T>
class Zasobnik
{
private:
    int index;
    T data[5];
public:
    Zasobnik() : index(0), data{}
    {

    }

    void push_back(T prvek)
    {
        data[index] = prvek;
        ++index;
    }

    T operator [] (int pozice)
    {
        T prvek = data[pozice];

        return prvek;
    }

    void pop_back()
    {
        --index;
    }

    T* begin()
    {
        return &data[0];
    }

    T* end()
    {
        return &data[index];
    }
};

int main()
{
    Zasobnik<int> zasobnik;

    zasobnik.push_back(1);
    zasobnik.push_back(2);
    zasobnik.push_back(3);

    int p1 = zasobnik[1];

    for (int prvek : zasobnik)
    {
        cout << prvek << endl;
    }

    Zasobnik<Zviratko*> zviratka;

    Pejsek pejsek;
    Kocicka kocicka;

    zviratka.push_back(&pejsek);
    zviratka.push_back(&kocicka);

    Zviratko* z = zviratka[1];

    for (Zviratko* zviratko : zviratka)
    {
        zviratko->Zvuk();
    }
    
    zviratka.pop_back();
}

