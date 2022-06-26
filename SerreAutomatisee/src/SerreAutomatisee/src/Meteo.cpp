#include "Meteo.h"

Meteo::Meteo(double p_temperature, double p_pourcentageHumidite, double p_pression) : temperature(p_temperature), pourcentageHumidite(p_pourcentageHumidite), pression(p_pression)
{
    ;
}

void Meteo::mettreAJour(Meteo p_nouvellesDonnes)
{
    this->temperature = p_nouvellesDonnes.temperature;
    this->pourcentageHumidite = p_nouvellesDonnes.pourcentageHumidite;
    this->pression = p_nouvellesDonnes.pression;
}
