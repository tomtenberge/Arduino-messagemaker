
void setup() {
  // put your setup code here, to run once:
  pinMode(SEin, OUTPUT);
  pinMode(BBin, OUTPUT);
  pinMode(A3in, OUTPUT);
  pinMode(A2in, OUTPUT);
  pinMode(A1in, OUTPUT);
  pinMode(A0in, OUTPUT);
  pinMode(GRin, OUTPUT);
  pinMode(CLKin, OUTPUT);
  pinMode(WEin, OUTPUT);
  pinMode(RDin, OUTPUT);
  pinMode(AEin, OUTPUT);
  pinMode(enBin, OUTPUT);
  Serial.begin(9600);
  load_card();
  init_screen_0();
}

void loop() {
  // put your main code here, to run repeatedly:
  updatescreen();
}
