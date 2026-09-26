class EdgeDetector
{
private:
  bool candidateState;
  unsigned long candidateSince;
  bool lastStableState;

  const bool targetState;
  const unsigned long minStableDuration;

public:
  EdgeDetector(bool targetState, unsigned long minStableDuration)
    : targetState(targetState), minStableDuration(minStableDuration)
  {

  }

  void begin(bool state, unsigned long time)
  {
    candidateState = state;
    candidateSince = time;
    lastStableState = state;
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

      if(duration > minStableDuration)
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

class DigitalOutput
{
private:
  const int pin;
  bool state;
    
  void writeState()
  {
    digitalWrite(pin, state);
  }

public:
  DigitalOutput(int pin) : pin(pin)
  {
  }

  void begin(bool state)
  {
    this->state = state;

    writeState();
  }

  void toggle()
  {
    state = !state;

    writeState();
  }
};

class DigitalPullupInput
{
private:
  const int pin;
public:
  DigitalPullupInput(int pin) : pin(pin)
  {
  }

  void begin()
  {
    pinMode(pin, INPUT_PULLUP);
  }

  bool read()
  {
    return digitalRead(pin);
  }
};

EdgeDetector edge(HIGH, 30);
DigitalOutput led(13);
DigitalPullupInput button(12);

void setup() 
{
  button.begin();
  edge.begin(button.read(), millis());
  led.begin(LOW);
}

void loop() 
{
  if(edge.detect(button.read(), millis()))
  {
    led.toggle();
  }
}