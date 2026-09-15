
// Prepocet z celsiu na ADC (0-1023) pro NTC termistor
// potrebuji jen pro konstantu limit
constexpr int thermistorCelsiusToAdc(float temp) 
{
    constexpr float VCC = 5.0;
    constexpr float SERIES_R = 10000.0;
    constexpr float NOM_R = 10000.0;
    constexpr float B_COEFF = 3950.0;
    constexpr float NOM_TEMP_K = 298.15;
    
    float tempK = temp +
     273.15;
    float r = NOM_R * exp(B_COEFF * ((1.0 / tempK) - (1.0 / NOM_TEMP_K)));
    float v = VCC * r / (SERIES_R + r);
    return (v / VCC) * 1023.0;
}

// pouze pro pripadny vypis do logu
float thermistorAdcToCelsius(int adc) 
{
  constexpr float VCC         = 5.0;
  constexpr float SERIES_R    = 10000.0;  // series resistor (10 kΩ)
  constexpr float NOM_R       = 10000.0;  // nominal resistance at 25 °C
  constexpr float B_COEFF     = 3950.0;   // Beta coefficient
  constexpr float NOM_TEMP_K  = 298.15;   // 25 °C in Kelvin

  float v    = adc * (VCC / 1023.0);
  float r    = SERIES_R * v / (VCC - v);
  float st   = log(r / NOM_R) / B_COEFF + 1.0 / NOM_TEMP_K;

  return (1.0 / st) - 273.15;
}

enum class Stav
{
  Init,
  Nizka,
  Vysoka
};

void setOutputs(Stav stav);

void setOutputs(Stav stav)
{
  switch(stav)
  {
    case Stav::Init:
      digitalWrite(13, LOW);
      break;

    case Stav::Nizka:
      digitalWrite(13, LOW);
      break;

    case Stav::Vysoka:
      digitalWrite(13, HIGH);
      break;
  }
}

// Poznámka: U NTC termistoru s odporem k VCC platí, že s rostoucí teplotou ADC hodnota klesá!  
constexpr int LIMIT_ADC = thermistorCelsiusToAdc(25.0);

constexpr unsigned long DEBOUNCE_MS = 200;

Stav stav       = Stav::Init;
Stav kandidat   = Stav::Init;
unsigned long kandidatOd = 0;

void setup() 
{
  Serial.begin(9600);
  delay(1000); // Serial.begin muze zpusobit druhy reset hodnot, na starsim UNO nemuzu overit ze uz probehl
  pinMode(13, OUTPUT);
  pinMode(A0, INPUT);
  setOutputs(Stav::Init);
}

void loop() 
{
  int adc = analogRead(A0);
 
  Stav novy_stav = stav;

  switch(stav)
  {
    case Stav::Init:
      if(adc < LIMIT_ADC)
      {
        novy_stav = Stav::Vysoka;
      }
      else
      {
        novy_stav = Stav::Nizka;
      }
      break;
    case Stav::Nizka:
      if(adc < LIMIT_ADC)
      {
        novy_stav = Stav::Vysoka;
      }
      break;

    case Stav::Vysoka:
      if(adc >= LIMIT_ADC)
      {
        novy_stav = Stav::Nizka;
      }
      break;
  }

  if (novy_stav != kandidat)      // kandidát se změnil -> restartuj odpočet
  {
    kandidat = novy_stav;
    kandidatOd = millis();
  }

  if (kandidat != stav && (millis() - kandidatOd) >= DEBOUNCE_MS)
  {
    stav = kandidat;
    setOutputs(stav);
  }

  delay(50);
}
