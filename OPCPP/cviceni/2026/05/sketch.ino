int pole[] = {0,6,24,53,88,128,167,202,231,249,255,249,231,202,167,128,88,53,24,6};
int delka = sizeof(pole) / sizeof(int);

int i = 0;

void setup() 
{
  pinMode(11, OUTPUT);
  analogWrite(11, 0);
}

void loop() 
{
  int intensity = pole[i];
  analogWrite(11, intensity);

  i = (i + 1) % delka;    

  delay(50);
}

//  0 % 20 =  0 - 0 * 20 =  0
// 19 % 20 = 19 - 0 * 20 = 19
// 20 % 20 = 20 - 1 * 20 =  0