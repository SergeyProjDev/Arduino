int port13 = 13;
int port8 = 8;

void setup(){
    pinMode(port13, OUTPUT);
    pinMode(port8, OUTPUT);
    tone(9,1318,500);
}
void loop(){
  digitalWrite(port13, HIGH);
  delay(1000);
  digitalWrite(port13, LOW);
  digitalWrite(port8, HIGH);
  delay(1000);
  digitalWrite(port8, LOW); 
}
