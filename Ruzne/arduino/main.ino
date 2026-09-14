#include "Led.h"

Led cervena(2); // Instance pro červenou LED na pinu 2
Led zelena(3);  // Instance pro zelenou LED na pinu 3

void setup() {
  cervena.inicializuj();
  zelena.inicializuj();
}

void loop() {
  cervena.zapni();
  zelena.vypni();
  delay(500);
  cervena.vypni();
  zelena.zapni();
  delay(500);
}
