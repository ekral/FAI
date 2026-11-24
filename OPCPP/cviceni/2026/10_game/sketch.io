#include <Wire.h>
#include <LiquidCrystal_I2C.h>

enum GameState
{
  LOTTERY_INIT,
  LOTTERY_RUNNING,
  LOTTERY_DISPLAY_WIN,
  LOTTERY_DISPLAY_TOTAL,
  NAME_ENTRY
};

struct Player
{
  char name[5] = {'A', 'A', 'A', 'A', '\0'};
  int score = 0;
};

enum PressState
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

  PressState detect(int state, int time)
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
            return (time - lastStateTime > 1500) ? LONG : SHORT;
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
constexpr int led_length = 16;
LiquidCrystal_I2C lcd(0x27, led_length, 2);

// eFSM

GameState state = LOTTERY_INIT;

// LOTTERY
constexpr int maxAttempts = 3;
constexpr int lotteryPeriod = 128;
constexpr int winDisplayDuration = 2000;

int roundCount = 1;
int wins[] = { 0,3,0,0,0,0,0,7,0,0,0,0,0,1,0,0};
int winIndex = 0;
int lastTime = 0;
int win = 0;
int totalWin = 0;

// NAME_ENTRY
Player player;
const int name_length = (sizeof(Player::name) / sizeof(char)) - 1;
int i = 0;

void setup() 
{
  lcd.init();
  lcd.backlight();
  lcd.cursor();
  
  buttonYellow.begin();
  buttonGray.begin();
  edgeYellow.begin(buttonYellow.read(), millis());
  edgeGray.begin(buttonGray.read(), millis());
  led.begin();
}

// Tohle mne resi entry akci
void ChangeState(GameState newState)
{
  switch(newState)
  {
    case LOTTERY_RUNNING:
    {
      lcd.setCursor(0, 0);

      for(int i = 0; i < led_length; i++)
      {
        int win = wins[i];

        if(win == 0)
        {
          lcd.print(' ');
        }
        else
        {
          lcd.print('+');
        }
      }

      lcd.setCursor(0, 1);
      lcd.print("round: ");
      lcd.print(roundCount); 
      lcd.print("       ");
      lcd.setCursor(0, 0);

      lastTime = millis();
      winIndex = 0;

      break;
    }

    case LOTTERY_DISPLAY_WIN:
    { 
      totalWin += win;

      lcd.setCursor(0, 1);
      lcd.print("total win: ");
      lcd.print(totalWin);
      lcd.setCursor(winIndex, 0);
      lcd.print(win);

      lastTime = millis();
      
      break;
    }

    case NAME_ENTRY:
    {
      for(int i = 0; i < name_length; i++)
      {
        player.name[i] = 'A';
      }

      player.score = 0; // TODO nastavit na win

      lcd.setCursor(0, 0);
      lcd.print(player.name);
      lcd.setCursor(0, 1);
      lcd.print(player.score);
      break;
    }

    default:
      break;
  }

  state = newState;
}

void nameEntry(PressState clickYellow, PressState clickGray)
{
  switch(clickYellow)
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
      lcd.setCursor(0, 0);
      ChangeState(LOTTERY_INIT);
      return;
      break;
    default:
      break;
  }

  if(clickGray == SHORT)
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

void loop() 
{
  PressState clickYellow = edgeYellow.detect(buttonYellow.read(), millis());
  PressState clickGray = edgeGray.detect(buttonGray.read(), millis());

  switch(state)
  {
    case LOTTERY_INIT:
    {
      roundCount = 1;
      totalWin = 0;
      ChangeState(LOTTERY_RUNNING);

      break;
    }

    case LOTTERY_RUNNING:
    {
      int duration = (millis() - lastTime);

      if(duration > lotteryPeriod)
      {
        ++winIndex;

        if(winIndex > led_length - 1)
        {
          winIndex = 0;
        }

        lcd.setCursor(winIndex, 0);

        lastTime = millis();
      }

      if(clickGray == SHORT)
      {
        win = wins[winIndex];
        ++roundCount;

        ChangeState(LOTTERY_DISPLAY_WIN);
      }

      break;
    }

    case LOTTERY_DISPLAY_WIN:
    {
      int duration = (millis() - lastTime);

      if(duration > winDisplayDuration)
      {
        ChangeState(LOTTERY_RUNNING);
      } 

      break;
    }

    case NAME_ENTRY:
    {
      nameEntry(clickYellow, clickGray);
      break;
    }
  }
}