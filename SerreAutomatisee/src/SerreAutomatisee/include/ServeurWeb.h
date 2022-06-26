#pragma once

#include <FS.h>
#include <WebServer.h>
#include <ArduinoJson.h>
#include "Serre.h"
#include "DonneesSerre.h"
#include "ConfigurationSerre.h"

class ServeurWeb
{
    public:
        ServeurWeb(Serre* p_serre);
        void tick();
        void testerSystemeFichierSPIFFS();

    private:
        WebServer* m_webServer;
        Serre* m_serre;
        const String nomFichierJSONConfigSerre = "/configurationSerre.json";
        void optionsCors() const;
        void envoyerCors() const;
        void afficherRacine();
        void ajouterFichiersStatiques(const String& p_debutNomFichier);
        void ajouterFichiersStatiques(const String& p_debutNomFichier, File& p_repertoire);
        void ressourceNonTrouvee(const String& p_nomFichier);
        void listFilesInDir(File p_dir, int p_numTabs = 1);
        void envoyerConfiguration();
        void modifierTemperatureDesiree(String p_temperatureJSON);
        void modifierControleAutomatique(String p_controleAutomatiqueJSON);
        void modifierPourcentageOuvertureManuel(String p_pourcentageOuvertureJSON);
        String recupererConfigurationJSON();
        ConfigurationSerre recupererConfigurationSerre();
        bool deserializerDansDocument(DynamicJsonDocument& p_document, String p_contenuJSON);
        void ecrireNouvelleConfigurationJSON(const DynamicJsonDocument p_documentJSON, String p_nomFichier);
};
