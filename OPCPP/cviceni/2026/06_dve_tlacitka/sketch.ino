struct EdgeDetector
{
private:
  const int minDuration = 30;
  const int targetState = HIGH;

  int candidate;
  int candidateSince;
  int lastState;

public:
  void begin(int state, int time)
  {
    candidate = state;
    candidateSince = time;
    lastState = state;
  }

  bool detect(int state, int time)
  {
    if(state != candidate)
    {
      candidate = state;
      candidateSince = time;
    }
    else
    {
      int duration = time - candidateSince;

      if(duration > minDuration)
      {
        if(candidate != lastState)
        {
          lastState = candidate;

          if(candidate == targetState)
          {
            return true;
          }
        }
      }
    }

    return false;

  }
};

// pridejte druhe tlacitko, ktere bude jas snizovat
// zamyslete jestli by sla udelat struktura i pro LED resp pro PWM vystup
EdgeDetector edgeRight;
EdgeDetector edgeLeft;
int ledPulseWidth = 32;

void setup() 
{
  pinMode(11, OUTPUT);
  pinMode(8, INPUT_PULLUP);
  pinMode(12, INPUT_PULLUP);

  analogWrite(11, ledPulseWidth);
  edgeRight.begin(digitalRead(8), millis);
  edgeLeft.begin(digitalRead(12), millis);
}

void loop() 
{
  if(edgeRight.detect(digitalRead(8), millis()))
  {
    ledPulseWidth += 8;
    analogWrite(11, ledPulseWidth);
  }

  if(edgeLeft.detect(digitalRead(12), millis()))
  {
    ledPulseWidth -= 8;
    analogWrite(11, ledPulseWidth);
  }
}