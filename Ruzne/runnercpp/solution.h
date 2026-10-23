struct AnalogInput
{
private:
    int pin;
public:
    AnalogInput(int pin) : pin(pin)
    {
        
    }

    int read()
    {
        return analogRead(pin);
    }
};

void KlientskyKod()
{
    AnalogInput analogInput(12);

    int value = analogInput.read();
}