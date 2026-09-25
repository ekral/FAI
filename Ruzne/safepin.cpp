// Obecná šablona (např. nahlásí chybu, pokud pin nepodporujeme)
template <uint8_t PIN_NUM>
class FastPin : public PinInterface<FastPin<PIN_NUM>> {
    // Generická implementace...
};

// Speciální implementace POUZE pro pin 13
template <>
class FastPin<13> : public PinInterface<FastPin<13>> {
public:
    FastPin() { DDRB |= (1 << 5); }
    void writeHigh() { PORTB |= (1 << 5); }
    void writeLow()  { PORTB &= ~(1 << 5); }
};
