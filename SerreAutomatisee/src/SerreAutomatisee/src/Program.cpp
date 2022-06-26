#include "Program.h"

Program::Program()
{
    this->m_configurationWifi = new ConfigurationWifi();
    this->m_serre = new Serre(this->m_configurationWifi);
    this->m_serveurWeb = new ServeurWeb(this->m_serre);
}

void Program::loop()
{
    this->m_serre->tick();
    this->m_serveurWeb->tick();
    this->m_configurationWifi->tick();
}
