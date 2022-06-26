# Serre automatisée

## Technologies utilisées

- C++
- Home Assistant
- MQTT Broker (PubSub)

## Description du programme
Projet final à caractère synthèse réalisé dans le cadre du cours applications mobiles et objets connectés (IOT) qui as été réalisé en équipe de 2. Nous devions nous servir de capteurs de température BME280 et leur trouver une utilité. Nous avons donc décidé de faire une serre en LEGO avec un toit dont l'ouverture est modulable. Le tout est contrôlé par un ESP32. Un moteur servo est attaché au panneau du toit pour ajuster l'angle selon le besoin. Les données météos peuvent être lues à partir de l'interface HomeAssistant ou de notre propre interface web (permet le contrôle de la serre). L'interface web as été développé pour mobile (non-responsive).

## Les différents façons de contrôler la serre (À partir d'une interface web pour mobile)
1. Mode automatique
- Inscrire la température désirée à l'intérieur de la serre.
  
L'angle du toit va s'ajuster automatiquement selon les conditions.


Si les conditions extérieures ne sont pas favorables, le toit sera complètement fermé.


Autrement, le toit s'ajustera selon l'écart entre la température actuelle et désirée.

2. Mode manuel

Possibilité d'ouvrir et fermer le toit. L'utilisateur peut aussi choisir le pourcentage d'ouverture.

### Dans les deux cas, il y a un affichage du pourcentage d'ouverture du toit et des températures intérieures et extérieures sur l'interface web.

## Vidéo youtube
https://youtu.be/y1TTOhf-8Bk
