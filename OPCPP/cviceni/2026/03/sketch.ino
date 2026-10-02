class EdgeDetector
{
private:
  bool candidateState;
  bool lastStableState;
  unsigned long candidateSince;
  bool targetState;

public:
  EdgeDetector(bool targetState) : targetState(targetState)
  {

  }

  void setup(bool state, unsigned long time)
  {
    candidateState = state;
    lastStableState = state;
    candidateSince = time;
  }

  bool detect(bool state, unsigned long time)
  {
    if(state != candidateState)
    {
      candidateState = state;
      candidateSince = time;
    }
    else
    {
      unsigned long duration = time - candidateSince;

      if(duration > 30)
      {
        if(candidateState != lastStableState)
        {
          lastStableState = candidateState;

          if(candidateState == targetState)
          {
            return true;
          }
        }
      }
    }

    return false;
  }
};

class InputPullupPin
{
private:
  int pinNumber;
public:
  InputPullupPin(int pinNumber) : pinNumber(pinNumber)
  {
  }

  void setup()
  {
    pinMode(pinNumber, INPUT_PULLUP);
  }

  bool read()
  {
    bool state = digitalRead(pinNumber);

    return state;
  }
};

class PwmOutputPin
{
private:
  int pinNumber;
  int pulseWidth;
  int step;
public:
  PwmOutputPin(int pinNumber, int pulseWidth, int step) 
    : pinNumber(pinNumber), pulseWidth(pulseWidth), step(step)
  {
  }

  void setup()
  {
    pinMode(pinNumber, OUTPUT);
  }

  void setPulseWidth()
  {
    analogWrite(pinNumber, pulseWidth);
  }

  void increasePulseWidth()
  {
    if(pulseWidth < 255 - step)
    {
      pulseWidth += step;
    }

    setPulseWidth();
  }

  void decreasePulseWidth()
  {
    if(pulseWidth > step)
    {
      pulseWidth -= step;
    }

    setPulseWidth();  
  }
};

InputPullupPin buttonL(12);
InputPullupPin buttonR(8);
PwmOutputPin led(9, 125, 5);

EdgeDetector edgeL(HIGH);
EdgeDetector edgeR(HIGH);

void setup() 
{
  led.setup();
  led.setPulseWidth();

  buttonL.setup();
  buttonR.setup();

  edgeL.setup(buttonL.read(), millis());
  edgeR.setup(buttonR.read(), millis());
}

void loop() 
{
  if(edgeL.detect(buttonL.read(), millis()))
  {
    led.decreasePulseWidth();
  }

  if(edgeR.detect(buttonR.read(), millis()))
  {
    led.increasePulseWidth();
  }
}