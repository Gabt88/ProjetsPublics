#pragma once

#include "CapteurTemperatureProxy.h"
#include "Meteo.h"

class CapteurTemperature
{
    public:
        CapteurTemperature(CapteurTemperatureProxy* p_proxy);
        double lireTemperature();
        double lirePourcentageHumidite();
        double lirePression();
        Meteo recupererMeteo();

    private:
        CapteurTemperatureProxy* m_proxy;
};
