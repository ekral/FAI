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
