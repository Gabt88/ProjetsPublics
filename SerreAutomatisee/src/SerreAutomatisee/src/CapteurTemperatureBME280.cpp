#include "CapteurTemperatureBME280.h"

CapteurTemperatureBME280::CapteurTemperatureBME280(int p_bus) : m_bus(p_bus)
{
    Wire.begin();
    this->m_bme280 = new Adafruit_BME280();
    this->changerBusI2C();
    if (!this->m_bme280->begin(0x76)) 
    {
        Serial.println("Erreur! Le capteur BME280 relié au bus " + String(this->m_bus) + " n'est pas reconnu.");
        while (1);
    }
    Serial.println();
}

double CapteurTemperatureBME280::lireTemperature()
{
    return this->m_bme280->readTemperature();
}

double CapteurTemperatureBME280::lirePourcentageHumidite()
{
    return this->m_bme280->readHumidity();
}

double CapteurTemperatureBME280::lirePression()
{
    return this->m_bme280->readPressure() / 100.0F;
}

void CapteurTemperatureBME280::changerBusI2C()
{
    Wire.beginTransmission(0x70);  // adresse du multiplexeur TCA9548A
    Wire.write(1 << this->m_bus);  // send byte to select bus
    Wire.endTransmission();
}
