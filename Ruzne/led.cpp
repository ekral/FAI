class Led {
  public:
    Led(uint8_t pin) : _pin(pin), _isOn(false) {}

    void begin() {
      pinMode(_pin, OUTPUT);
      digitalWrite(_pin, LOW);
    }

    void on() {
      _isOn = true;
      digitalWrite(_pin, HIGH);
    }

    void off() {
      _isOn = false;
      digitalWrite(_pin, LOW);
    }

    bool isOn() const {
      return _isOn;
    }

  private:
    uint8_t _pin;
    bool _isOn;
};