// 1. Třída SafePin (používá klasické digitalWrite)
class SafePin {
private:
    uint8_t _pin;
public:
    SafePin(uint8_t pin) : _pin(pin) {
        pinMode(_pin, OUTPUT);
    }

    void turnOn() {
        digitalWrite(_pin, HIGH);
    }

    void turnOff() {
        digitalWrite(_pin, LOW);
    }
};

// 2. Třída FastPin (používá přímý zápis, C++11 specializace pro Pin 13)
template <uint8_t PIN_NUM>
class FastPin {
    // Obecná šablona může zůstat prázdná nebo vyvolat static_assert
};

// Specializace šablony specificky pro Arduino Uno Pin 13
template <>
class FastPin<13> {
public:
    FastPin() {
        DDRB |= (1 << 5); // Nastaví Pin 13 jako výstup
    }

    void turnOn() {
        PORTB |= (1 << 5); // Přímý zápis HIGH
    }

    void turnOff() {
        PORTB &= ~(1 << 5); // Přímý zápis LOW
    }
};

// --- 3. Statický polymorfismus v praxi (Šablonová funkce) ---
// Funkce přijme jakýkoliv typ T, který má metody turnOn() a turnOff()
template <typename T>
void blink(T& pin) {
    pin.turnOn();
    delay(500);
    pin.turnOff();
    delay(500);
}

// --- Použití ---

SafePin ledSafe(12);   // Dynamický pin 12 (lze změnit za běhu)
FastPin<13> ledFast;   // Statický pin 13 (vše vyřešeno při kompilaci)

void setup() {}

void loop() {
    blink(ledSafe);    // Kompilátor vygeneruje verzi funkce blink() pro SafePin
    blink(ledFast);    // Kompilátor vygeneruje druhou verzi funkce blink() pro FastPin<13>
}
