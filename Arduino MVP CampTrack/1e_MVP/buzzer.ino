/*
   Buzzer module
*/

void initBuzzer()
{
  pinMode(buzzer, OUTPUT);
}

void beep(int seconds, int height)
{
  tone(buzzer, height);
  delay(seconds);
  noTone(buzzer);
}

void multipleBeep(int count, int seconds, int height)
{
  int i = 0;
  while (i < count)
  {
    tone(buzzer, height);
    delay(seconds);
    noTone(buzzer);
    i++;
  }
}
