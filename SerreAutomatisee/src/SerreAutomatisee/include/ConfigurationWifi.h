#pragma once
#include <WiFi.h>
#include <WiFiManager.h>
#include <Arduino.h>

class ConfigurationWifi
{
    private:
       WiFiManager m_wm;
       IPAddress m_adresseIPPortail;
       IPAddress m_passerellePortail;
       IPAddress m_masqueReseauPortail;
       char const* SSIDPortail = "ConfigurationSerre";
       char const* motPasseAPPortail = "Bonjour01.+";
       int const DelaisSansConnexion = 300000;
       unsigned long m_dateDerniereConnexion;
       bool m_estConnecte;
    public:
        ConfigurationWifi();
        const char* obtenirAdresseIp();
        void tick();

        
};
