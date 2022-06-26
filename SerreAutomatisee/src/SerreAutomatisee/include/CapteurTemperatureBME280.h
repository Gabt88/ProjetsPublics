#pragma once

#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include "CapteurTemperatureProxy.h"

class CapteurTemperatureBME280 : public CapteurTemperatureProxy
{
    public:
        CapteurTemperatureBME280(int p_bus);
        double lireTemperature();
        double lirePourcentageHumidite();
        double lirePression();
        void changerBusI2C();

    private:
        Adafruit_BME280* m_bme280;
        int m_bus;
};
