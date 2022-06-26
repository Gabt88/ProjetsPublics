 #include "ClientMqtt.h"
 
ClientMqtt::ClientMqtt()
{
  this->m_client = new PubSubClient(this->m_espClient);
  this->m_client->setServer(this->adresse_serveur_mqtt, this->port_serveur_mqtt);
  this->connecter();
}

bool ClientMqtt::connecter()
{
  bool estConnecte = this->m_client->connected();

  if (!estConnecte)
  {
    int nbEssaisActuel = 0;
    const int nbEssaisMax = 5;

    while (!estConnecte && nbEssaisActuel != nbEssaisMax)
    {
      if (this->m_client->connect("ESP32", this->utilisateurMqtt, this->motPasseMqtt))
      {
        Serial.println("Connecté à MQTT!");
        estConnecte = true;
      }

      else 
      {
        Serial.println("Échec de la connexion à MQTT ...");
        Serial.print(this->m_client->state());
        nbEssaisActuel++;
      }
    }
  }

  return estConnecte;
}

void ClientMqtt::publierDonneesMeteo(Meteo* p_meteoInterieure, Meteo* p_meteoExterieure)
{
  char temperatureInterieure[7];
  char pourcentageHumiditeInterieur[7];
  char pressionInterieure[7];

  dtostrf(p_meteoInterieure->temperature, 5, 2, temperatureInterieure);
  dtostrf(p_meteoInterieure->pourcentageHumidite, 2, 0, pourcentageHumiditeInterieur);
  dtostrf(p_meteoInterieure->pression, 4, 0, pressionInterieure);

  char temperatureExterieure[7];
  char pourcentageHumiditeExterieur[7];
  char pressionExterieure[7];
  
  dtostrf(p_meteoExterieure->temperature, 5, 2, temperatureExterieure);
  dtostrf(p_meteoExterieure->pourcentageHumidite, 2, 0, pourcentageHumiditeExterieur);
  dtostrf(p_meteoExterieure->pression, 4, 0, pressionExterieure);

  this->m_client->publish(this->topic_tempInterieure, temperatureInterieure);
  this->m_client->publish(this->topic_humiditeInterieure, pourcentageHumiditeInterieur);
  this->m_client->publish(this->topic_pressionInterieure, pressionInterieure);

  this->m_client->publish(this->topic_tempExterieure, temperatureExterieure);
  this->m_client->publish(this->topic_humiditeExterieure, pourcentageHumiditeExterieur);
  this->m_client->publish(this->topic_pressionExterieure, pressionExterieure);
}

void ClientMqtt::publierAdresseIP(const char* p_adresseIP)
{
  this->m_client->publish(this->topic_adresseIP, p_adresseIP);
}
