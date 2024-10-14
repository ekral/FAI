# Přetěžování funkcí a operátorů

**autor: Erik Král ekral@utb.cz**

---

# Přetěžování funkcí

Přetěžování funkcí znamená, že můžeme mít více funkcí se stejným názvem, pokud mají jiné parametry. V následujícím příkladu máme tři metody `Vypis`. První metoda má jeden parametr typu `int`, druhá dva parametry typu `int` a třetí má parametr typu `double`. Překladač potom podle typu argumentu zavolá správnou metodu `Vypis`.

```cpp
void Vypis(int x)
{
    printf("int x: %d\n", x);
}

void Vypis(int x, int y)
{
    printf("int x: %d, int y: %d\n", x, y);
}

void Vypis(double x)
{
    printf("double x: %f\n", x);
}

int main()
{
    Vypis(3);
    Vypis(3, 4);
    Vypis(3.0);

    return 0;
}
```

# Přetěžování operátorů

Přetěžování operátorů znamená, že kromě metod můžou třídy a struktury podporovat i operátory.

# Kopírovací a přesouvací konstruktor

Kopírovací a přesouvací konstruktor slouží k vytvoření hluboké kopie instance třídy respektive struktury.
