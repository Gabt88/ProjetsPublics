#pragma once

class DonneesSerre
{
    public:
        DonneesSerre(double p_temperatureInterieure, double p_temperatureExterieure, int p_pourcentageOuvertureToit);
        double temperatureInterieure;
        double temperatureExterieure;
        int pourcentageOuvertureToit;
};
