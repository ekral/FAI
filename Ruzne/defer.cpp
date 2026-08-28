// 1. DEFINICE STRÁŽCE (Studenti stačí, když pochopí, že destruktor ~ se spustí VŽDY na konci)
template <typename F>
class ErrDefer {
private:
    F cleanupFunc;      // Kód, který se má spustit při chybě
    bool active = true; // Výchozí stav: strážce je aktivní a hlídá chybu

public:
    ErrDefer(F f) : cleanupFunc(f) {}

    // Destruktor: Spustí se automaticky při opuštění bloku/funkce
    ~ErrDefer() { 
        if (active) {
            cleanupFunc(); 
        } 
    }

    // Metoda pro "potvrzení" úspěchu – zruší spuštění úklidu
    void commit() { 
        active = false; 
    }
};



bool inicializujSenzor() {
    // KROK 1: Zapneme napájení senzoru
    digitalWrite(PIN_NAPAJENI, HIGH);
    
    // Hned pod to nasadíme strážce chyb. 
    // Pokud funkce skončí předčasně (přes return false), napájení se vypne.
    ErrDefer guard = []() { 
        digitalWrite(PIN_NAPAJENI, LOW); 
        Serial.println("Úklid: Inicializace selhala, vypínám napájení.");
    };

    // KROK 2: Pokusíme se probudit senzor přes sběrnici
    if (!Senzor.probudSe()) {
        return false; // <- Zde funkce končí. guard zaniká, volá se destruktor -> vypne napájení.
    }

    // KROK 3: Pokusíme se načíst kalibrační data
    if (!Senzor.nactiKalibraci()) {
        return false; // <- I zde by guard zachránil situaci a vypnul napájení.
    }

    // KROK 4: Vše proběhlo v pořádku! 
    // Potvrdíme úspěch. Tím guardu řekneme "vše je OK, nic neuklízej".
    guard.commit(); 
    
    Serial.println("Senzor úspěšně spuštěn!");
    return true; // <- Funkce končí, guard zaniká, ale díky commit() nic neudělá.
}
