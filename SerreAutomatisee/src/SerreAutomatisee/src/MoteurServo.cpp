#include "MoteurServo.h"

MoteurServo::MoteurServo(MoteurServoProxy* p_proxy) : m_proxy(p_proxy)
{
    ;
}

void MoteurServo::ajusterAngle(int p_pourcentageOuvertureToit)
{
    this->m_proxy->ajusterAngle(p_pourcentageOuvertureToit);
}
