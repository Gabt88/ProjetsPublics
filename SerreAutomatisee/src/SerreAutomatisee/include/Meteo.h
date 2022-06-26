#pragma once

class Meteo
{
    public:
        double temperature;
        double pourcentageHumidite;
        double pression;

        Meteo(double p_temperature, double p_pourcentageHumidite, double p_pression);
        void mettreAJour(Meteo p_nouvellesDonnes);
};
