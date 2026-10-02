#define LED_PIN 9
#define BUTTONL_PIN 12
#define BUTTONR_PIN 8

int ledIntensity;

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
Button buttonR;

void setup() 
{
  pinMode(LED_PIN, OUTPUT);
  analogWrite(LED_PIN, ledIntensity);

  pinMode(BUTTONL_PIN, INPUT_PULLUP);
  pinMode(BUTTONR_PIN, INPUT_PULLUP);

  buttonL.setup(digitalRead(BUTTONL_PIN), millis());
  buttonR.setup(digitalRead(BUTTONR_PIN), millis());
}

void loop() 
{
  if(buttonL.update(digitalRead(BUTTONL_PIN), millis()))
  {
    if(ledIntensity > 5)
    {
      ledIntensity -= 5;
    }

    analogWrite(LED_PIN, ledIntensity);
  }

  if(buttonR.update(digitalRead(BUTTONR_PIN), millis()))
  {
    if(ledIntensity < 250)
    {
      ledIntensity += 5;
    }

    analogWrite(LED_PIN, ledIntensity);
  }
}