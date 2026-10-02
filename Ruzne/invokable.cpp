#include <iostream>
#include <concepts>

// Definice třídy omezené konceptem std::invocable
// F musí být volatelné s argumenty (int, double)
template <typename F>
requires std::invocable<F, int, double>
struct MojeTrida {
    const F akce;

    // Constexpr konstruktor s členským inicializačním listem
    constexpr MojeTrida(F zadanaAkce) : akce(zadanaAkce) {}

    constexpr void Spust(int a, double b) const {
        akce(a, b); // Plně inlinováno a zoptimalizováno
    }
};

int main() {
    // 1. Správné použití: Lambda přijímá int a double
    constexpr auto instance = MojeTrida([](int x, double y) {
        std::cout << "OK: " << x << " a " << y << "\n";
    });
    instance.Spust(10, 3.14);

    // 2. CHYBA PŘI PŘEKLADU (pokud odkomentujete):
    // Lambda nesplňuje koncept std::invocable pro (int, double)
    /*
    constexpr auto chyba = MojeTrida([](std::string s) {
        std::cout << s << "\n";
    });
    */
}
