#include <Wire.h>
#include <LiquidCrystal_I2C.h>

struct EdgeDetector
{
private:
  const int minDuration;
  const bool targetState;

  int candidate;
  int candidateSince;
  int lastState;

public:
  EdgeDetector(int minDuration, bool targetState)
    : minDuration(minDuration), targetState(targetState)
  {

  }

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

struct PwmOutput
{
private:
  const int pin;
  int pulseWidth;
  const int step;

  void write()
  {
    analogWrite(pin, pulseWidth);
  }

public:
  PwmOutput(int pin, int pulseWidth, int step)
    : pin(pin), pulseWidth(pulseWidth), step(step)
  {
  }

  void begin()
  {
    pinMode(pin, OUTPUT);
    write();
  }

  void increase()
  {
    pulseWidth += step;
    write();
  }

  void decrease()
  {
    pulseWidth -= step;
    write();
  }
};

struct DigitalInputPullup
{
private:
  int pin;
public:
  DigitalInputPullup(int pin) : pin(pin)
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

DigitalInputPullup buttonRight(8);
DigitalInputPullup buttonLeft(12);
EdgeDetector edgeRight(30, HIGH);
EdgeDetector edgeLeft(30, HIGH);
PwmOutput led(11, 64, 8);
LiquidCrystal_I2C lcd(0x27, 16, 2);

char jmeno[] = "PTR";
int i = 0;

void setup() 
{
  lcd.init();
  lcd.backlight();
  lcd.setCursor(0,0);
  lcd.cursor();
  lcd.print(jmeno);

  buttonRight.begin();
  buttonLeft.begin();
  edgeRight.begin(buttonRight.read(), millis());
  edgeLeft.begin(buttonLeft.read(), millis());
  led.begin();
}

void loop() 
{
  if(edgeRight.detect(buttonRight.read(), millis()))
  {
    ++i;
    
    if(i > 2)
    {
      i = 0;
    }

    lcd.setCursor(i,0);
  }

  if(edgeLeft.detect(buttonLeft.read(), millis()))
  {
    ++jmeno[i];
    
    if(jmeno[i] > 'Z')
    {
      jmeno[i] = 'A';
    }

    lcd.setCursor(i,0);
    lcd.print(jmeno[i]);
  }
}