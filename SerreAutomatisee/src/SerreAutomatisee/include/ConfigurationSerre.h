#pragma once

class ConfigurationSerre
{
    public:
        ConfigurationSerre(bool p_controleAutomatique, double p_temperatureInterieureDesiree, int p_pourcentageOuvertureToitModeManuel);
        bool controleAutomatique;
        double temperatureInterieureDesiree;
        int pourcentageOuvertureToitModeManuel;
};
