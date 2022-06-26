#pragma once

#include "MoteurServoProxy.h"

class MoteurServo
{
    public:
        MoteurServo(MoteurServoProxy* p_proxy);
        void ajusterAngle(int p_pourcentageOuvertureToit);

    private:
        MoteurServoProxy* m_proxy;
};
