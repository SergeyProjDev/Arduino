//Libraries
#include <DHT.h>;
#include <Adafruit_Sensor.h>

//Constants
#define DHTPIN 7     // what pin we're connected to
#define DHTTYPE DHT22   // DHT 22  (AM2302)
DHT dht(DHTPIN, DHTTYPE); //// Initialize DHT sensor for normal 16mhz Arduino


//Variables
int chk;
float hum;  //Stores humidity value
float temp; //Stores temperature value

void setup()
{
  Serial.begin(9600);
  dht.begin();
}

char c;
void loop()
{
  c = Serial.read();
  if (c == '1') {
    hum = dht.readHumidity();
    temp= dht.readTemperature();
    Serial.print(hum);
    Serial.print("   ");
    Serial.println(temp);
    Serial.write('0');
  }
}
