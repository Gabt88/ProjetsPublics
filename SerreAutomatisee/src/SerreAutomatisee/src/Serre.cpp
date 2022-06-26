#include "Serre.h"

#define DEBUG false
#define DEBUG_MQTT false
#define PIN_SERVO 18
#define BUS_CAPTEUR_TEMP_INTERIEUR 2
#define BUS_CAPTEUR_TEMP_EXTERIEUR 4

Serre::Serre(ConfigurationWifi* p_configurationWifi) : m_configurationWifi(p_configurationWifi), m_pourcentageOuvertureToit(0), m_temperatureDesiree(0), m_controleAutomatique(true) // Le ServeurWeb va s'occuper d'injecter la configuration (fichierJSON) dans la serre lors de sa création
{
    this->m_clientMqtt = new ClientMqtt();

    MoteurServoMG996R* moteurServoMG996R = new MoteurServoMG996R(PIN_SERVO);

    this->m_moteurControleToit = new MoteurServo(moteurServoMG996R);
    CapteurTemperatureBME280* capteurBME280TemperatureInterieur = new CapteurTemperatureBME280(BUS_CAPTEUR_TEMP_INTERIEUR);
    CapteurTemperatureBME280* capteurBME280TemperatureExterieur = new CapteurTemperatureBME280(BUS_CAPTEUR_TEMP_EXTERIEUR);
    this->m_capteurTemperatureInterieur = new CapteurTemperature(capteurBME280TemperatureInterieur);
    this->m_capteurTemperatureExterieur = new CapteurTemperature(capteurBME280TemperatureExterieur);

    Meteo meteoActuelleInterieure = this->m_capteurTemperatureInterieur->recupererMeteo();
    this->m_meteoInterieure = new Meteo(meteoActuelleInterieure.temperature, meteoActuelleInterieure.pourcentageHumidite, meteoActuelleInterieure.pression);
    Meteo meteoActuelleExterieure = this->m_capteurTemperatureExterieur->recupererMeteo();
    this->m_meteoExterieure = new Meteo(meteoActuelleExterieure.temperature, meteoActuelleExterieure.pourcentageHumidite, meteoActuelleExterieure.pression);

    this->m_dateDerniereMiseAJourMeteo = millis();
}

void Serre::tick()
{
    unsigned long dateActuelle = millis();

    if (dateActuelle - this->m_dateDerniereMiseAJourMeteo >= frequenceMiseAJourMeteoMs)
    { 
        // Meteo Interieure
        this->mettreAJourMeteoInterieure();

        if (DEBUG)
        {
            Serial.println("***** Meteo Interieure *****");
            Serial.print("Temperature = ");
            Serial.print(this->m_meteoInterieure->temperature);
            Serial.println(" *C");
            
            Serial.print("Pression = ");
            Serial.print(this->m_meteoInterieure->pression);
            Serial.println(" hPa");

            Serial.print("Humidite = ");
            Serial.print(this->m_meteoInterieure->pourcentageHumidite);
            Serial.println(" %");
            Serial.println();
        }              

        // Meteo Exterieure
        this->mettreAJourMeteoExterieure();

        if (DEBUG)
        {
            Serial.println("***** Meteo Exterieure *****");
            Serial.print("Temperature = ");
            Serial.print(this->m_meteoExterieure->temperature);
            Serial.println(" *C");
            
            Serial.print("Pression = ");
            Serial.print(this->m_meteoExterieure->pression);
            Serial.println(" hPa");

            Serial.print("Humidite = ");
            Serial.print(this->m_meteoExterieure->pourcentageHumidite);
            Serial.println(" %");

            Serial.println();
        }

        if (this->m_controleAutomatique)
        {
            this->ajusterPourcentageOuvertureToit(this->calculerPourcentageOuvertureToitModeAutomatique());
        }

        if (this->m_clientMqtt->connecter())
        {
            if (DEBUG_MQTT)
            {
                Serial.println("Publication des données sur HA");
            }

            this->m_clientMqtt->publierDonneesMeteo(this->m_meteoInterieure, this->m_meteoExterieure);
            this->m_clientMqtt->publierAdresseIP(this->m_configurationWifi->obtenirAdresseIp());
        }

        this->m_dateDerniereMiseAJourMeteo = millis();
    }
}

void Serre::activerControleAutomatique(bool p_vraiOuFaux)
{
    this->m_controleAutomatique = p_vraiOuFaux;
}

void Serre::ajusterTemperatureDesiree(double p_temperatureDesiree)
{
    this->m_temperatureDesiree = p_temperatureDesiree;
}

void Serre::ajusterPourcentageOuvertureToit(int p_pourcentageOuverture)
{
    this->m_pourcentageOuvertureToit = p_pourcentageOuverture;
    this->m_moteurControleToit->ajusterAngle(this->m_pourcentageOuvertureToit);
}

void Serre::mettreAJourMeteoInterieure()
{
    this->m_meteoInterieure->mettreAJour(this->m_capteurTemperatureInterieur->recupererMeteo());
}

void Serre::mettreAJourMeteoExterieure()
{
    this->m_meteoExterieure->mettreAJour(this->m_capteurTemperatureExterieur->recupererMeteo());
}

double Serre::calculerPourcentageOuvertureToitModeAutomatique()
{
    int pourcentageOuvertureToit;
    double temperatureInterieure = this->m_meteoInterieure->temperature;
    double temperatureExterieure = this->m_meteoExterieure->temperature;
    double temperatureDesiree = this->m_temperatureDesiree;

    if ((temperatureInterieure < temperatureDesiree && temperatureInterieure > temperatureExterieure) || (temperatureInterieure > temperatureDesiree && temperatureInterieure < temperatureExterieure))
    {
        pourcentageOuvertureToit = 0;
    }

    else 
    {
        double differenceAbsolueTemperatureInterieurVsDesiree = temperatureInterieure >= temperatureDesiree ? (temperatureInterieure - temperatureDesiree) : (temperatureDesiree - temperatureInterieure);

        if (differenceAbsolueTemperatureInterieurVsDesiree > 0 && differenceAbsolueTemperatureInterieurVsDesiree <= 2)
        {
            pourcentageOuvertureToit = 20;
        }

        else if (differenceAbsolueTemperatureInterieurVsDesiree > 2 && differenceAbsolueTemperatureInterieurVsDesiree <= 4)
        {
            pourcentageOuvertureToit = 40;
        }

        else if (differenceAbsolueTemperatureInterieurVsDesiree > 4 && differenceAbsolueTemperatureInterieurVsDesiree <= 6)
        {
            pourcentageOuvertureToit = 60;
        }

        else if (differenceAbsolueTemperatureInterieurVsDesiree > 6 && differenceAbsolueTemperatureInterieurVsDesiree <= 8)
        {
            pourcentageOuvertureToit = 80;
        }

        else 
        {
            pourcentageOuvertureToit = 100;
        }
    }

    return pourcentageOuvertureToit;
}

DonneesSerre Serre::recupererDonnees()
{
    return DonneesSerre(this->m_meteoInterieure->temperature, this->m_meteoExterieure->temperature, this->m_pourcentageOuvertureToit);
}
