/*
   Delay Module
*/



void clearLcd(int input) {
  if (millis() - previousTime > input)
  {
    lcd.clear();
    clearLCD = false;
  }
}
