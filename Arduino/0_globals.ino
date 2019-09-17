//set pins for display control
#define SEin 2
#define BBin 3
#define A3in 4
#define A2in 5
#define A1in 6
#define A0in 7
#define GRin 8
#define CLKin 9
#define WEin A0
#define RDin A1
#define AEin A2
#define enBin A3
#define SD_Select
#include <SPI.h>
#include <SD.h>
bool bbstatus = 0;
int slides[9][2];
int currentslideshow = 0;
int currentslide = 0;
//init screen
void init_screen_0()
{
  int a = 0;
  int i = 0;
  int j = 0;
  digitalWrite(enBin,1);
  digitalWrite(SEin,1);
  if (bbstatus == true)
  {
    bbstatus = false;
  }
  else
  {
    bbstatus = true;
  }
  digitalWrite(BBin,bbstatus);
  for (i = 0;i<16;i++){
    digitalWrite(A0in,bitRead(i,0));
    digitalWrite(A1in,bitRead(i,1));
    digitalWrite(A2in,bitRead(i,2));
    digitalWrite(A3in,bitRead(i,3));
    digitalWrite(AEin,1);
    digitalWrite(WEin,1);
    digitalWrite(WEin,0);
    digitalWrite(AEin,0);
    for (j = 0;j<128;j++)
    {
      digitalWrite(RDin,0);
      digitalWrite(GRin,0);
      digitalWrite(CLKin,1);
      digitalWrite(CLKin,0);
    }
  }
}
