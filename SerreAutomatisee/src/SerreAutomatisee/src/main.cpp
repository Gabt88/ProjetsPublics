#include <Arduino.h>
#include "Program.h"

Program* programme;

void setup() {
  Serial.begin(115200);
  programme = new Program();
}

void loop() {
  programme->loop();
}