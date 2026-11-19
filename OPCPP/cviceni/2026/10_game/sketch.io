#include <Wire.h>
#include <LiquidCrystal_I2C.h>

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

enum GameState
{
  INIT,
  GAME,
  NAME_ENTRY,
  TOP_PLAYERS
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
LiquidCrystal_I2C lcd(0x27, 16, 2);

Player player;
const int name_length = (sizeof(Player::name) / sizeof(char)) - 1;
int i = 0;

int indexes[10];
Player players[10];

int loot[16] = { 0, 0, 100, 0, 0, 50, 0, 0, 20, 0, 0, 10, 0, 0, 30, 0}; 

GameState gameState = INIT;
int game_index = 0;
int game_period = 40;
int game_last_time;

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

GameState init_game()
{
  lcd.setCursor(0, 0);

  for(int i = 0; i < 16; i++)
  {
    char c = loot[i] == 0 ?  ' ' : '$';
    lcd.print(c);
  }

  return GAME;
}

void nameEntry()
{
  PressState pressState = edgeYellow.detect(buttonYellow.read(), millis());

  switch(pressState)
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
      lcd.print("Long press");
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

GameState game()
{
  int duration = millis() - game_last_time;

  if(duration > game_period)
  {
    ++game_index;
    
    if(game_index > 15)
    {
      game_index = 0;
    }

    game_last_time = millis();

    lcd.setCursor(game_index, 0);
  }

  PressState pressState = edgeYellow.detect(buttonYellow.read(), millis());

  if(pressState == SHORT)
  {
    int win = loot[game_index];

    lcd.setCursor(0, 1);
    lcd.print(win);
    lcd.print("     ");
    lcd.setCursor(game_index, 0);

    return NAME_ENTRY;
  }

  return GAME;
}

void loop() 
{
  GameState nextState;

  switch(gameState)
  {
    case INIT:
      nextState = init_game();
      break;
    case GAME:
      nextState = game();
      break;
    case NAME_ENTRY:
      break;
    case TOP_PLAYERS:
      break;
  }

  gameState = nextState;
  
}