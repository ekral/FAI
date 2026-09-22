bool ledState = LOW;
bool candidateButtonState = HIGH;
unsigned long candidateTimeChanged = 0; // Opraven datový typ
bool lastStableButtonState = HIGH;

void setup() {
  delay(1000);
  pinMode(13, OUTPUT);
  pinMode(12, INPUT_PULLUP);

  candidateButtonState = digitalRead(12);
  lastStableButtonState = candidateButtonState;
  candidateTimeChanged = millis();
}

void loop() {
  bool buttonState = digitalRead(12);
  
  if(buttonState != candidateButtonState)   
  {
    candidateTimeChanged = millis();
    candidateButtonState = buttonState;
  }

  unsigned long candidateDuration = millis() - candidateTimeChanged;

  if(candidateDuration > 30)   
  {
    bool stableButtonState = candidateButtonState;

    if(stableButtonState != lastStableButtonState)     
    {
      if(stableButtonState == HIGH)
      {
        ledState = !ledState;
        digitalWrite(13, ledState);
      }

      lastStableButtonState = stableButtonState;
    }
  }
}
