#pragma once

class CapteurTemperatureProxy
{
    public:
        virtual double lireTemperature() = 0;
        virtual double lirePourcentageHumidite() = 0;
        virtual double lirePression() = 0;
        virtual void changerBusI2C() = 0;
};
