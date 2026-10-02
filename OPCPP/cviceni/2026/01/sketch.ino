#define LED_PIN 13
#define BUTTON_PIN 12

bool ledState = LOW;
bool buttonCandidateState;
bool lastButtonStableState;

unsigned long candidateTime;

void setup() 
{
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  digitalWrite(LED_PIN, ledState);

  buttonCandidateState = digitalRead(BUTTON_PIN);
  lastButtonStableState = buttonCandidateState;
  candidateTime = millis();
}

void loop() 
{
  bool buttonState = digitalRead(BUTTON_PIN);

  if(buttonState != buttonCandidateState)
  {
    buttonCandidateState = buttonState;
    candidateTime = millis();
  }
  else
  {
    unsigned long duration = millis() - candidateTime;

    if(duration > 30)
    {
      bool buttonStableState = buttonCandidateState;

      if(buttonStableState != lastButtonStableState)
      {
        lastButtonStableState = buttonStableState;

        if(buttonStableState == HIGH)
        {
          ledState = !ledState;
          digitalWrite(LED_PIN, ledState);
        }
      }
    }
  }
}
