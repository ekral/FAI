// Blink LED Example
// Toggles the built-in LED on pin 13

void setup() {
  Serial.begin(9600);
  delay(1000);

  pinMode(13, OUTPUT);
  pinMode(12, INPUT_PULLUP);
}

bool lastButtonState = HIGH;
bool candidateButtonState = HIGH;
unsigned long candidateTimeChanged = 0;
bool candidateLedState = HIGH;

void loop() {
  int buttonState = digitalRead(12);

  if(buttonState != candidateButtonState)
  {
    candidateButtonState = buttonState;
    candidateTimeChanged = millis();
  }

  unsigned long candidateDuration = millis() - candidateTimeChanged;

  if(candidateDuration > 30)
  {
    if(candidateButtonState != lastButtonState)
    {
      if(candidateButtonState == HIGH)
      {
        digitalWrite(13, candidateLedState);

        candidateLedState = !candidateLedState;
      }

       lastButtonState = buttonState;
    }
  }
}