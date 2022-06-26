#include <ConfigurationWifi.h>

ConfigurationWifi::ConfigurationWifi() : m_adresseIPPortail(192, 168, 23, 1), m_passerellePortail(192, 168, 23, 1), m_masqueReseauPortail(255, 255, 255, 0), m_dateDerniereConnexion(0)
{
    this->m_wm.setConfigPortalTimeout(180);
     
    this->m_wm.setAPStaticIPConfig(m_adresseIPPortail, m_passerellePortail,
                         m_masqueReseauPortail);
    
    this->m_estConnecte = this->m_wm.autoConnect(SSIDPortail, motPasseAPPortail);
    
    if(!this->m_estConnecte)
    {
        Serial.println("Connexion échouée au démarrage du programme");
        ESP.restart();
    }
}

void ConfigurationWifi::tick()
{
    if(WiFi.isConnected())
    {
        this->m_dateDerniereConnexion = millis();
    }

    if(millis() - this->m_dateDerniereConnexion > this->DelaisSansConnexion)
    {
        Serial.println("Perte de connexion! Redémarrage du ESP32");
        ESP.restart();
    }
}

const char* ConfigurationWifi::obtenirAdresseIp()
{
    return WiFi.localIP().toString().c_str();
}
