// Blink LED Example
// Toggles the built-in LED on pin 13

void setup() {
  pinMode(13, OUTPUT);
  pinMode(12, INPUT_PULLUP);
}

bool ledState = LOW;
bool lastButtonStableState = HIGH;
bool candidateButtonState = HIGH;
unsigned long int candidateTimeChanged = 0; 
// unsigned jsou cisla bez znameka, 0, 1, .. long int zabere 4 byty (32 bitu), 2 ^ 32

void loop() {

  bool buttonState = digitalRead(12);

  if(buttonState != candidateButtonState)
  {
    candidateButtonState = buttonState;
    candidateTimeChanged = millis(); // millis() vrati aktualni cas
  }

  unsigned long int candidateDuration = millis() - candidateTimeChanged;

  if(candidateDuration > 30)
  {
      int stableButtonState = candidateButtonState; // candidate bereme uz jako stable

      if(stableButtonState != lastButtonStableState)
      {
        if(stableButtonState == HIGH)
        {
          ledState = !ledState;
          digitalWrite(13, ledState);
        }
      }

      lastButtonStableState = stableButtonState;
  }

}
