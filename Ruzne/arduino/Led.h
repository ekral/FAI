// Soubor: Led.h
#include <Arduino.h>

class Led {
  private:
    int pin; // Zapouzdřený atribut - nikdo zvenčí k němu nemůže

  public:
    // Konstruktor
    Led(int cisloPinu) {
      pin = cisloPinu;
    }

    // Metoda pro inicializaci hardwaru
    void inicializuj() {
      pinMode(pin, OUTPUT);
    }

    // Akční metody
    void zapni() {
      digitalWrite(pin, HIGH);
    }

    void vypni() {
      digitalWrite(pin, LOW);
    }
};
