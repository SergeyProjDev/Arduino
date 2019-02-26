int led = 8;

void setup() {
      Serial.begin(9600);
      pinMode(led, OUTPUT);           
}
char c;
void loop() {
    c = Serial.read();
    if (c == '1') {
      digitalWrite(led, HIGH);
    }
    if (c == '2') {
      digitalWrite(led, LOW);
    }    
}
