//CampTrack MVP2.0
//Fabian van Hekke

//Includes
#include <PN532.h>
#include <SPI.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#define PN532_CS 10
PN532 nfc(PN532_CS);
LiquidCrystal_I2C lcd(0x27, 16, 2);
#define  NFC_DEMO_DEBUG 1

//Inputs
const int PIN_BUTTON_1 = 3;
const int PIN_BUTTON_2 = 2;
const int buzzer = 9;

//Variabele voor Delays
unsigned long previousTime = 0;
int waitOne = 1000;
int waitFive = 5000;
int waitTen = 10000;
bool clearLCD = false;




void setup(void) {
#ifdef NFC_DEMO_DEBUG
  Serial.begin(9600);
  Serial.println("CampTrack MVP - Startup");
  initButtons();
  initBuzzer();
  lcd.begin();
  lcd.backlight();


#endif
  nfc.begin();
  uint32_t versiondata = nfc.getFirmwareVersion();
  if (! versiondata) {
    while (1); // halt
  }
  nfc.SAMConfig();
}

void loop(void) {
  uint32_t id;
  id = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A);

  if (id != 0)
  {
#ifdef NFC_DEMO_DEBUG
    beep(500, 500);
    Serial.print("CardNumber:");
    Serial.println(id);
#endif

  }
  serialread();

  if (buttonPressed(PIN_BUTTON_1))
  {
    delay(50);
    beep(200, 600);
    Serial.println("Option:Changed");
    lcd.setCursor(7, 1);
    lcd.print("Bevestigd");
    previousTime = millis();
    clearLCD = true;

  }
  if (buttonPressed(PIN_BUTTON_2))
  {
    delay(50);
    beep(200, 600);
    Serial.println("Next");
    lcd.print("Volgende");
    previousTime = millis();
    clearLCD = true;

  }

  if (clearLCD)
  {
    clearLcd(waitFive);
  }


}
