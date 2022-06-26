#include "CapteurTemperature.h"

CapteurTemperature::CapteurTemperature(CapteurTemperatureProxy* p_proxy) : m_proxy(p_proxy)
{
    ;
}

double CapteurTemperature::lireTemperature()
{
    return this->m_proxy->lireTemperature();
}

double CapteurTemperature::lirePourcentageHumidite()
{
    return this->m_proxy->lirePourcentageHumidite();
}

double CapteurTemperature::lirePression()
{
    return this->m_proxy->lirePression();
}

Meteo CapteurTemperature::recupererMeteo()
{
    this->m_proxy->changerBusI2C();
    return Meteo(this->lireTemperature(), this->lirePourcentageHumidite(), this->lirePression());
}
