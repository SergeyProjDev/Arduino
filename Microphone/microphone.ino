const int sensorPin = A0; 
int sensorValue = 0;
int led = 8;
boolean flag = true;


void setup() {
  Serial.begin(9600);
  pinMode(sensorPin, INPUT); 
  pinMode(led, OUTPUT) ; 
}

void loop() {
  sensorValue = analogRead(sensorPin);
  Serial.println(sensorValue);
  if (sensorValue > 35 ) {
     if (flag){
       digitalWrite (led, HIGH);
       delay (500);
       flag = false;
     }
     else{
       digitalWrite (led, LOW);
       delay (500);
       flag = true;
     }
  }
}
