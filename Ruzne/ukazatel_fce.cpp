#include <iostream>

// Definice typu pro ukazatel na funkci (bere int, vrací bool)
using FunkcePtr = bool(*)(int);

struct MojeTrida {
    const FunkcePtr akce; // Obyčejný ukazatel na funkci

    // Constexpr konstruktor s členským inicializačním listem
    constexpr MojeTrida(FunkcePtr zadanaAkce) : akce(zadanaAkce) {}

    constexpr bool Spust(int x) const {
        return akce(x); 
    }
};

// Cílová funkce známá v době překladu
bool JeKladne(int x) {
    return x > 0;
}

int main() {
    // Vytvoření constexpr instance třídy s předáním funkce
    constexpr MojeTrida instance(JeKladne);

    // Volání v době překladu (ověřeno pomocí static_assert)
    static_assert(instance.Spust(5) == true);
    static_assert(instance.Spust(-3) == false);

    std::cout << "Vse funguje v dobe prekladu!\n";
}
