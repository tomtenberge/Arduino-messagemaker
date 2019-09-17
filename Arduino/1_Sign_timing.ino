String makefilename()
{
  String filename = "";
  if (currentslideshow < 10)
  {
    filename = filename + "0";
  }
  filename = filename + (String)currentslideshow;
  if (currentslide < 10)
  {
    filename = filename + "0";
  }
  filename = filename + (String)currentslide;
  filename = filename + ".MMS";
  Serial.println(filename);
  return filename;
}

void updatescreen() {
  currentslide++;
  if (currentslide > slides[currentslideshow][0])
  {
    currentslide = 0;
  }
  int a = 0;
  int i = 0;
  int j = 0;
  digitalWrite(enBin,1);
  digitalWrite(SEin,1);
  if (bbstatus == true){
    bbstatus = false;
  }
  else
  {
    bbstatus = true;
  }
  digitalWrite(BBin,bbstatus);
  File datafile = SD.open(makefilename(), FILE_READ);
  for (i = 0;i<16;i++){
    int p = i - 1;
    if (p == -1)
    {
      p = 15;
    }
    digitalWrite(A0in,bitRead(p,0));
    digitalWrite(A1in,bitRead(p,1));
    digitalWrite(A2in,bitRead(p,2));
    digitalWrite(A3in,bitRead(p,3));
    digitalWrite(AEin,1);
    digitalWrite(WEin,1);
    digitalWrite(WEin,0);
    digitalWrite(AEin,0);
    for (j = 0;j<128;j++){
      switch(datafile.read()){
        case 0:
          digitalWrite(RDin,0);
          digitalWrite(GRin,0);
        break;
        case 1:
          digitalWrite(RDin,0);
          digitalWrite(GRin,1);
        break;
        case 2:
          digitalWrite(RDin,1);
          digitalWrite(GRin,0);
        break;
        case 3:
          digitalWrite(RDin,1);
          digitalWrite(GRin,1);
        break;
        default:
          digitalWrite(RDin,0);
          digitalWrite(GRin,0);
      }
      digitalWrite(CLKin,1);
      digitalWrite(CLKin,0);
    }
  }
  datafile.close();
}
