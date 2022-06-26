#pragma once

#include "Serre.h"
#include "ServeurWeb.h"
#include "ConfigurationWifi.h"

class Program
{
    public:
        Program();
        void loop();

    private:
        Serre* m_serre;
        ServeurWeb* m_serveurWeb;
        ConfigurationWifi* m_configurationWifi;
};