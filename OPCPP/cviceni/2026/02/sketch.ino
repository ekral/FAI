#define LED_PIN 13
#define BUTTON_PIN 12

bool ledState = LOW;

struct Button
{
  bool buttonCandidateState;
  bool lastButtonStableState;
  unsigned long candidateTime;

  void setup(bool state, unsigned long time)
  {
    buttonCandidateState = state;
    lastButtonStableState = state;
    candidateTime = time;
  }

  bool update(bool state, unsigned long time)
  {
    if(state != buttonCandidateState)
    {
      buttonCandidateState = state;
      candidateTime = time;
    }
    else
    {
      unsigned long duration = time - candidateTime;

      if(duration > 30)
      {
        bool buttonStableState = buttonCandidateState;

        if(buttonStableState != lastButtonStableState)
        {
          lastButtonStableState = buttonStableState;

          if(buttonStableState == HIGH)
          {
            return true;
          }
        }
      }
    }

    return false;
  }
};

Button button;

void setup() 
{
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  digitalWrite(LED_PIN, ledState);

  button.setup(digitalRead(BUTTON_PIN), millis());
}

void loop() 
{
  unsigned long time = millis();

  if(button.update(digitalRead(BUTTON_PIN), time))
  {
    ledState = !ledState;
    digitalWrite(LED_PIN, ledState);
  }
}