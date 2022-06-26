#include "MoteurServoMG996R.h"

#define DEBUG false

MoteurServoMG996R::MoteurServoMG996R(uint8_t p_pin) : m_pin(p_pin)
{
    this->m_servoMG996R = new Servo();
    this->m_servoMG996R->attach(this->m_pin);
    this->m_servoMG996R->setPeriodHertz(50);
}

void MoteurServoMG996R::ajusterAngle(int p_pourcentageOuvertureToit)
{
    int pourcentage = constrain(p_pourcentageOuvertureToit, 0, 100);
    int angle = (pourcentage * angleToitCompletementOuvert) / 100;

    if (DEBUG)
    {
        Serial.println(angle);
    }

    this->m_servoMG996R->write(angle);
}
