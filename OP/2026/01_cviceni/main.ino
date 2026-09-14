
float tempC(int raw) 
{
  constexpr float VCC         = 5.0;
  constexpr float SERIES_R    = 10000.0;  // series resistor (10 kΩ)
  constexpr float NOM_R       = 10000.0;  // nominal resistance at 25 °C
  constexpr float B_COEFF     = 3950.0;   // Beta coefficient
  constexpr float NOM_TEMP_K  = 298.15;   // 25 °C in Kelvin

  float v    = raw * (VCC / 1023.0);
  float r    = SERIES_R * v / (VCC - v);
  float st   = log(r / NOM_R) / B_COEFF + 1.0 / NOM_TEMP_K;

  return (1.0 / st) - 273.15;
}

enum class Stav
{
  Nizka,
  Vysoka
};

void setOutputs(Stav stav);

void setOutputs(Stav stav)
{
  switch(stav)
  {
    case Stav::Nizka:
      digitalWrite(13, LOW);
      break;

    case Stav::Vysoka:
      digitalWrite(13, HIGH);
      break;
  }
}

Stav stav = Stav::Nizka;

void setup() 
{
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  pinMode(A0, INPUT);
}

void loop() 
{
  int raw = analogRead(A0);
  float teplota = tempC(raw);

  constexpr float limit = 25.0;

  Stav novy = stav;

  switch(stav)
  {
    case Stav::Nizka:
      if(teplota > limit)
      {
        novy = Stav::Vysoka;
      }
      break;

    case Stav::Vysoka:
      if(teplota <= limit)
      {
        novy = Stav::Nizka;
      }
      break;
  }

  if(novy != stav)
  {
    stav = novy;
    setOutputs(stav);
  }
}