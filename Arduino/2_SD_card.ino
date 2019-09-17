void load_card()
{
  Serial.print("Initializing SD card...");
  if (!SD.begin(10)) {
    Serial.println("initialization failed!");
    while (1);
  }
  File configfile = SD.open("INFO.MMI", FILE_READ);
  for (int i = 0;i<10;i++)
  {
    int test;
    if (((int)configfile.read()) == i)
    {
      slides[i][0] = (int)configfile.read();
      slides[i][1] = (int)configfile.read();
    }
    else
    {
      Serial.println("SD card data incorrect");
    }
  }
  configfile.close();
  Serial.println("initialization done.");
}
