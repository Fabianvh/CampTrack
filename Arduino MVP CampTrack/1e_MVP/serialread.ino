/*
  Serial Read Module
*/

void serialread()
{
  String receiveVal;
  String client = "Client:";
  String licensePlate = "LicensePlate:";
  String state = "State:";
  //String error = "Error:";

  if (Serial.available() > 0)
  {
    receiveVal = Serial.readString();

    if (receiveVal.indexOf(client) + client.length()) //getallen omzetten naar .length
    {
      String clientName = receiveVal.substring(receiveVal.indexOf(client) + client.length(), receiveVal.indexOf(licensePlate));
      lcd.setCursor (0, 0);
      lcd.print(clientName);
    }

    if (receiveVal.indexOf(licensePlate) + licensePlate.length())
    {
      String licensePlt = receiveVal.substring(receiveVal.indexOf(licensePlate) + licensePlate.length(), receiveVal.indexOf(state));
      lcd.setCursor (0, 1);
      lcd.print(licensePlt);
    }

    if (receiveVal.indexOf(state) + state.length())
    {
      String parked = receiveVal.substring(receiveVal.indexOf(state) + state.length());
      if (parked.equals("False"))
      {
        lcd.setCursor (8, 1);
        lcd.print("Stallen?");
      } else if (parked.equals("True"))
      {
        lcd.setCursor (8, 1);
        lcd.print("Ophalen?");
      }
    }
  }
}
