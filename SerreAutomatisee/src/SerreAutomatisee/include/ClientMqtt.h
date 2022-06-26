#pragma once

#include <WiFiClient.h>
#include <PubSubClient.h>
#include <Wire.h>
#include "Meteo.h"

class ClientMqtt 
{
    public:
        ClientMqtt();
        bool connecter();
        void publierDonneesMeteo(Meteo* p_meteoInterieure, Meteo* p_meteoExterieure);
        void publierAdresseIP(const char* p_adresseIP);

    private:
        WiFiClient m_espClient;
        PubSubClient* m_client;
        const char* utilisateurMqtt = "mqtt";
        const char* motPasseMqtt = "mqtt";
        const char* adresse_serveur_mqtt = "192.168.2.61";
        const int port_serveur_mqtt = 1883;
        const char* topic_adresseIP = "adresseIP";
        const char* topic_tempInterieure = "tempInterieure";
        const char* topic_humiditeInterieure = "humiditeInterieure";
        const char* topic_pressionInterieure = "pressionInterieure";
        const char* topic_tempExterieure = "tempExterieure";
        const char* topic_humiditeExterieure = "humiditeExterieure";
        const char* topic_pressionExterieure = "pressionExterieure";
};
