#include <SPIFFS.h>
#include <uri/UriRegex.h>
#include "ServeurWeb.h"

#define DEBUG false

ServeurWeb::ServeurWeb(Serre* p_serre) : m_serre(p_serre)
{
    SPIFFS.begin();
    this->m_webServer = new WebServer();
    this->ajouterFichiersStatiques("/");

    this->m_webServer->on(Uri("/"), HTTPMethod::HTTP_GET,
                        [this]() { this->afficherRacine(); });

    this->m_webServer->on(Uri("/serre"), HTTPMethod::HTTP_GET,
                        [this]() { this->envoyerConfiguration(); });

    this->m_webServer->on(Uri("/modifTempDesiree"), HTTPMethod::HTTP_PUT,
                        [this]() { this->modifierTemperatureDesiree( this->m_webServer->arg("plain")); });

    this->m_webServer->on(Uri("/modifControleAuto"), HTTPMethod::HTTP_PUT,
                        [this]() { this->modifierControleAutomatique( this->m_webServer->arg("plain")); });

    this->m_webServer->on(Uri("/modifPourcentageOuvertureManuel"), HTTPMethod::HTTP_PUT,
                        [this]() { this->modifierPourcentageOuvertureManuel( this->m_webServer->arg("plain")); });

    this->m_webServer->on(UriRegex(".*"), HTTPMethod::HTTP_OPTIONS,
                        [this]() { this->optionsCors(); });  

    this->m_webServer->onNotFound(
        [this]() { this->ressourceNonTrouvee(this->m_webServer->uri()); });

    ConfigurationSerre configSerre = this->recupererConfigurationSerre();

    this->m_serre->activerControleAutomatique(configSerre.controleAutomatique);
    this->m_serre->ajusterTemperatureDesiree(configSerre.temperatureInterieureDesiree);
    
    if (!configSerre.controleAutomatique)
    {
        this->m_serre->ajusterPourcentageOuvertureToit(configSerre.pourcentageOuvertureToitModeManuel);
    }

    this->m_webServer->begin();
}

void ServeurWeb::tick()
{
    this->m_webServer->handleClient();
}

void ServeurWeb::testerSystemeFichierSPIFFS()
{
    delay(500);

    Serial.println(F("Inizializing FS..."));

    if (SPIFFS.begin())
    {
        Serial.println(F("SPIFFS mounted correctly."));
    }

    else
    {
        Serial.println(F("!An error occurred during SPIFFS mounting"));
    }

    // Get all information of SPIFFS
    unsigned int totalBytes = SPIFFS.totalBytes();
    unsigned int usedBytes = SPIFFS.usedBytes();

    Serial.println("===== File system info =====");

    Serial.print("Total space:      ");
    Serial.print(totalBytes);
    Serial.println("byte");

    Serial.print("Total space used: ");
    Serial.print(usedBytes);
    Serial.println("byte");

    Serial.println();

    // Open dir folder
    File dir = SPIFFS.open("/");
    // List file at root
    listFilesInDir(dir);
}

void ServeurWeb::optionsCors() const
{
    this->m_webServer->sendHeader("Access-Control-Allow-Origin", "*");
    this->m_webServer->sendHeader("Access-Control-Max-Age", "600");
    this->m_webServer->sendHeader("Access-Control-Allow-Methods", "PUT,POST,GET,OPTIONS");
    this->m_webServer->sendHeader("Access-Control-Allow-Headers", "*");
    this->m_webServer->send(204);
}

void ServeurWeb::envoyerCors() const
{
    this->m_webServer->sendHeader("Access-Control-Allow-Origin", "*");
}

void ServeurWeb::afficherRacine()
{
    this->envoyerCors();
    this->m_webServer->sendHeader("Location", "index.html", true);
    this->m_webServer->send(301, "text/plain", "");
}

void ServeurWeb::ajouterFichiersStatiques(const String& p_debutNomFichier)
{
    File racine = SPIFFS.open("/");
    ajouterFichiersStatiques(p_debutNomFichier, racine);
}

void ServeurWeb::ajouterFichiersStatiques(const String& p_debutNomFichier, File& p_repertoire)
{
    if (!p_repertoire)
    {
        return;
    }

    Serial.println(String("Traitement du répertoire: ") + p_repertoire.name());
    File fichier = p_repertoire.openNextFile();

    while (fichier) 
    {
        String nomFichier = String(fichier.name());

        if (fichier.isDirectory()) 
        {
            ajouterFichiersStatiques(p_debutNomFichier, fichier);
        } 
        
        else 
        {
            if (nomFichier.startsWith(p_debutNomFichier)) 
            {
                Serial.println(String("Ajout du fichier statique: " + nomFichier));
                this->m_webServer->serveStatic(nomFichier.c_str(), SPIFFS, nomFichier.c_str());
            }
        }

        fichier.close();
        fichier = p_repertoire.openNextFile();
    }

    p_repertoire.close();
}

void ServeurWeb::ressourceNonTrouvee(const String& p_nomFichier)
{
    Serial.println("Ressource '" + p_nomFichier + "' non trouvée !");
    this->m_webServer->send(404, "text/plain", "Ressource '" + p_nomFichier + "' non trouvée !");
}

void ServeurWeb::listFilesInDir(File p_dir, int p_numTabs)
{
    while (true) 
    {
        File entry =  p_dir.openNextFile();
        if (! entry) 
        {
            break; // no more files in the folder
        }

        for (uint8_t i = 0; i < p_numTabs; i++) 
        {
            Serial.print('\t');
        }

        Serial.print(entry.name());

        if (entry.isDirectory()) 
        {
            Serial.println("/");
            listFilesInDir(entry, p_numTabs + 1);
        } 

        else 
        {
            // display zise for file, nothing for directory
            Serial.print("\t\t");
            Serial.println(entry.size(), DEC);
        }

        entry.close();
    }
}

void ServeurWeb::envoyerConfiguration()
{
    ConfigurationSerre configurationFichierJSON = this->recupererConfigurationSerre();
    DonneesSerre donnees = this->m_serre->recupererDonnees();
    DynamicJsonDocument doc(1024);
    JsonObject configuration = doc.createNestedObject("configuration");

    configuration["controleAutomatique"] = configurationFichierJSON.controleAutomatique;
    configuration["temperatureInterieureDesiree"] = configurationFichierJSON.temperatureInterieureDesiree;
    configuration["pourcentageOuvertureToitModeManuel"] = configurationFichierJSON.pourcentageOuvertureToitModeManuel;

    if (configurationFichierJSON.controleAutomatique == true)
    {
        configuration["pourcentageOuvertureToitModeAutomatique"] = donnees.pourcentageOuvertureToit;
    }

    configuration["temperatureInterieure"] = donnees.temperatureInterieure;
    configuration["temperatureExterieure"] = donnees.temperatureExterieure;

    char configurationJSONFinale[1024];
    serializeJson(doc, configurationJSONFinale);

    if (DEBUG)
    {
        Serial.println(configurationJSONFinale);
    }
    
    this->envoyerCors();
    this->m_webServer->send(200, "application/json", configurationJSONFinale);
}

void ServeurWeb::modifierTemperatureDesiree(String p_temperatureJSON)
{
    Serial.println("PUT température");
    DynamicJsonDocument docTemperature(1024);
    bool aucuneErreur = deserializerDansDocument(docTemperature, p_temperatureJSON);

    this->envoyerCors();

    if (!aucuneErreur)
    {
        this->m_webServer->send(400);
    }

    else 
    {
        DynamicJsonDocument docConfiguration(1024);
        deserializerDansDocument(docConfiguration, this->recupererConfigurationJSON());
        double temperatureInterieureDesiree = docTemperature["temperatureInterieureDesiree"];
        docConfiguration["configuration"]["temperatureInterieureDesiree"] = temperatureInterieureDesiree;
        this->m_serre->ajusterTemperatureDesiree(temperatureInterieureDesiree);
        this->ecrireNouvelleConfigurationJSON(docConfiguration, this->nomFichierJSONConfigSerre);
        this->m_webServer->send(200);
    }
}

void ServeurWeb::modifierControleAutomatique(String p_controleAutomatiqueJSON)
{
    Serial.println("PUT controle automatique");
    DynamicJsonDocument docControleAutomatique(1024);
    bool aucuneErreur = deserializerDansDocument(docControleAutomatique, p_controleAutomatiqueJSON);

    this->envoyerCors();
    
    if (!aucuneErreur)
    {
        this->m_webServer->send(400);
    }

    else 
    {
        DynamicJsonDocument docConfiguration(1024);
        deserializerDansDocument(docConfiguration, this->recupererConfigurationJSON());
        bool estControleAutomatiquement = docControleAutomatique["controleAutomatique"];
        docConfiguration["configuration"]["controleAutomatique"] = estControleAutomatiquement;
        this->m_serre->activerControleAutomatique(estControleAutomatiquement);

        if (!estControleAutomatiquement)
        {
            ConfigurationSerre configActuelle = this->recupererConfigurationSerre();
            this->m_serre->ajusterPourcentageOuvertureToit(configActuelle.pourcentageOuvertureToitModeManuel);
        }
    
        this->ecrireNouvelleConfigurationJSON(docConfiguration, this->nomFichierJSONConfigSerre);
        this->m_webServer->send(200);
    }
}

void ServeurWeb::modifierPourcentageOuvertureManuel(String p_pourcentageOuvertureJSON)
{
    Serial.println("PUT pourcentage ouverture");
    DynamicJsonDocument docPourcentageOuverture(1024);
    bool aucuneErreur = deserializerDansDocument(docPourcentageOuverture, p_pourcentageOuvertureJSON);

    this->envoyerCors();
    
    if (!aucuneErreur)
    {
        this->m_webServer->send(400);
    }

    else 
    {
        DynamicJsonDocument docConfiguration(1024);
        deserializerDansDocument(docConfiguration, this->recupererConfigurationJSON());
        int pourcentageOuvertureToitModeManuel = docPourcentageOuverture["pourcentageOuvertureToitModeManuel"];
        docConfiguration["configuration"]["pourcentageOuvertureToitModeManuel"] = pourcentageOuvertureToitModeManuel;
        this->m_serre->ajusterPourcentageOuvertureToit(pourcentageOuvertureToitModeManuel);
        this->ecrireNouvelleConfigurationJSON(docConfiguration, this->nomFichierJSONConfigSerre);
        this->m_webServer->send(200);
    }
}

String ServeurWeb::recupererConfigurationJSON()
{
    File fichierJSON = SPIFFS.open(this->nomFichierJSONConfigSerre);
    String contenuJSON = "";

    if (fichierJSON) 
    {
        while (fichierJSON.available())
        {
            contenuJSON += char(fichierJSON.read());
        }
    }

    fichierJSON.close();

    return contenuJSON;
}

ConfigurationSerre ServeurWeb::recupererConfigurationSerre()
{
    String configurationJSON = this->recupererConfigurationJSON();

    DynamicJsonDocument doc(1024);
    DeserializationError error = deserializeJson(doc, configurationJSON);

    if (error)
    {
        Serial.print(F("deserializeJson() failed: "));
        Serial.println(error.f_str());
    }

    return ConfigurationSerre(doc["configuration"]["controleAutomatique"], doc["configuration"]["temperatureInterieureDesiree"], doc["configuration"]["pourcentageOuvertureToitModeManuel"]);
}

bool ServeurWeb::deserializerDansDocument(DynamicJsonDocument& p_document, String p_contenuJSON)
{
    DeserializationError erreur = deserializeJson(p_document, p_contenuJSON);
    return !erreur;
}

void ServeurWeb::ecrireNouvelleConfigurationJSON(const DynamicJsonDocument p_documentJSON, String p_nomFichier)
{
    if (SPIFFS.exists(p_nomFichier))
    {
        SPIFFS.remove(p_nomFichier);
    }

    File fichierJSON = SPIFFS.open(p_nomFichier, FILE_WRITE);

    if (fichierJSON)
    {
        char chaineTmp[1024];
        serializeJson(p_documentJSON, chaineTmp);
        fichierJSON.print(chaineTmp);
    }

    fichierJSON.close();
}
