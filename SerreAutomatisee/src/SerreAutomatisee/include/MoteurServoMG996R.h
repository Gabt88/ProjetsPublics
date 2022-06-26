#pragma once

#include <Arduino.h>
#include <ESP32Servo.h>
#include "MoteurServoProxy.h"

class MoteurServoMG996R : public MoteurServoProxy
{
    public:
        MoteurServoMG996R(uint8_t p_pin);
        void ajusterAngle(int p_pourcentageOuvertureToit);

    private:
        Servo* m_servoMG996R;
        uint8_t m_pin;
        const int angleToitCompletementFerme = 0;
        const int angleToitCompletementOuvert = 130;
};
