#include <Wire.h>
#include <LiquidCrystal_I2C.h>

struct Player
{
  char name[5] = { 'A', 'A', 'A', 'A', '\0'};
  int score = 128;
};

constexpr int name_length = (sizeof(Player::name) / sizeof(char)) - 1;

enum PressType
{
  NONE,
  SHORT,
  LONG
};

struct EdgeDetector
{
private:
  const int minDuration;
  const bool targetState;

  int candidate;
  int candidateSince;
  int lastState;
  int lastStateTime;

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
    lastStateTime = time;
  }

  PressType detect(int state, int time)
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
            int pressDuration = time - lastStateTime;

            PressType type = pressDuration > 1500 ? LONG : SHORT;

            return type;
          }

          lastStateTime = time;
        }
      }
    }

    return NONE;
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

DigitalInputPullup buttonYellow(8);
DigitalInputPullup buttonGray(12);
EdgeDetector edgeYellow(30, HIGH);
EdgeDetector edgeGray(30, HIGH);
PwmOutput led(11, 64, 8);
LiquidCrystal_I2C lcd(0x27, 16, 2);

Player player;

int i = 0;

void setup() 
{
  Serial.begin(9600);
  delay(1000);

  lcd.init();
  lcd.backlight();
  lcd.cursor();
  lcd.setCursor(0,1);
  lcd.print(player.score);
  lcd.setCursor(0,0);
  lcd.print(player.name);

  buttonYellow.begin();
  buttonGray.begin();
  edgeYellow.begin(buttonYellow.read(), millis());
  edgeGray.begin(buttonGray.read(), millis());
  led.begin();
}

void loop() 
{
  PressType pressType = edgeYellow.detect(buttonYellow.read(), millis());

  switch(pressType)
  {
    case SHORT:
      ++i;
    
      if(i > name_length - 1)
      {
        i = 0;
      }

      lcd.setCursor(i, 0);
      break;
    case LONG:
      lcd.setCursor(0, 1);
      lcd.print("SAFE");
      break;
    default:
      break;
  }

  if(edgeGray.detect(buttonGray.read(), millis()) == SHORT)
  {
    ++player.name[i];
    
    if(player.name[i] > 'Z')
    {
      player.name[i] = 'A';
    }

    lcd.setCursor(i, 0);
    lcd.print(player.name[i]);
  }
}
