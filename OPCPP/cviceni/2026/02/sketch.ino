#define LED_PIN 13
#define BUTTONL_PIN 12
#define BUTTONR_PIN 8

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

Button buttonL;

void setup() 
{
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, ledState);

  pinMode(BUTTONL_PIN, INPUT_PULLUP);
  pinMode(BUTTONR_PIN, INPUT_PULLUP);

  buttonL.setup(digitalRead(BUTTONL_PIN), millis());
}

void loop() 
{
  unsigned long time = millis();

  if(buttonL.update(digitalRead(BUTTONL_PIN), time))
  {
    ledState = !ledState;
    digitalWrite(LED_PIN, ledState);
  }
}