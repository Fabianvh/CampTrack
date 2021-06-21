/*
   Button Module
*/



void initButtons()
{
  pinMode(PIN_BUTTON_1, INPUT_PULLUP);
  pinMode(PIN_BUTTON_2, INPUT_PULLUP);
}

bool buttonPressed(int buttonPin)
{
  bool buttonPressed = false;
  if (digitalRead(buttonPin) == LOW)
  {
    delay(50);
    while (digitalRead(buttonPin) == LOW);
    buttonPressed = true;
  }
  return buttonPressed;
}
