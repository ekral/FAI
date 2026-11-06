struct Trida
{
public:
    int x;

    Trida(int x) : x(x)
    {
        
    }
};

// nechat zapoznamkovane
/*
void print(const Trida* trida)
{
    Serial.print("X: ");
    Serial.println(trida->x);
}
*/

void setup(int x)
{
    Trida* pt = new Trida(health);
    print(pt);
    delete pt;
}