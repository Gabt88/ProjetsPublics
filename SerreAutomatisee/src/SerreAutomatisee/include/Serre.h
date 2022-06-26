#pragma once

#include "CapteurTemperature.h"
#include "CapteurTemperatureBME280.h"
#include "MoteurServo.h"
#include "MoteurServoMG996R.h"
#include "Meteo.h"
#include "DonneesSerre.h"
#include "ClientMqtt.h"
#include "ConfigurationWifi.h"

class Serre 
{
    public:
        Serre(ConfigurationWifi* p_configurationWifi);
        void tick();
        void ajusterPourcentageOuvertureToit(int p_pourcentageOuverture);
        void ajusterTemperatureDesiree(double p_temperatureDesiree);
        void activerControleAutomatique(bool p_vraiOuFaux);
        DonneesSerre recupererDonnees();

    private:
        ClientMqtt* m_clientMqtt;
        ConfigurationWifi* m_configurationWifi;
        CapteurTemperature* m_capteurTemperatureInterieur;
        CapteurTemperature* m_capteurTemperatureExterieur;
        Meteo* m_meteoInterieure;
        Meteo* m_meteoExterieure;
        MoteurServo* m_moteurControleToit;
        int m_pourcentageOuvertureToit;
        double m_temperatureDesiree;
        unsigned long m_dateDerniereMiseAJourMeteo;
        const unsigned long frequenceMiseAJourMeteoMs = 2000;
        bool m_controleAutomatique;
        void mettreAJourMeteoInterieure();
        void mettreAJourMeteoExterieure();
        double calculerPourcentageOuvertureToitModeAutomatique();
};
