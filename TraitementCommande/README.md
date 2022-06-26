# Traitement de commandes générées aléatoirements

## Technologies utilisées
- C#
- RabbitMQ (PubSub)

## Description du programme
Le but de l'exercice est de simuler un système de gestion de commandes. Un message "commande placée" est généré et quatre consommateurs le lis et effectue un traitement différent (journalisation, préparation de la facture, préparation de l'expédition, envoie d'un courriel dans le cas d'une commande premium).

## Les projets
1. DSED_M07_TraitementCommande_Producteur (Producteur)
  
- Ce programme va émettre des messages "Commande placée" dans un échange nommé "m07-commandes" en utilisant les sujets suivants : "commande.placee.normal" et "commande.placee.premium".

- Une commande contient un nom de client, une référence de commande, une liste d'articles. Un article a une référence, un nom, un prix et une quantité.

- Les messages envoyés contiennent un objet composé des informations de la commande sérialisée en JSON.

2. DSED_M07_TraitementCommande_Journal (Consommateur)

- Il est abonné à tous les sujets.

- À la réception d'un message, il enregistre le contenu du message dans un fichier dont le nom débute par l'année, le mois, le jour, l'heure, le nombre de minutes, le nombre de secondes et un numéro unique avec le format suivant : "AAAAMMJJ_HHMMSS_Nouveau_Guid.json".

3. DSED_M07_TraitementCommande_Facturation (Consommateur)

- Il est abonné à tous les sujets débutants par "commande.placee".

- À la réception d'un message, il calcule le montant de la facture. Si la commande est de catégorie premium, il enlève 5%. Il calcule aussi les taxes avec les taux actuels appliqués dans la province du Québec. La liste des articles, ainsi que le total sans et avec taxes sont enregistrés dans un fichier par commande dont le nom débute par par l'année, le mois, le jour, l'heure, le nombre de minutes, le nombre de secondes et la référence de la commande avec le format suivant : "AAAAMMJJ_HHMMSS_ReferenceCommande_Facture.json"

4. DSED_M07_TraitementCommande_Expedition (Consommateur)

- Il est abonné à tous les sujets débutants par "commande.placee".

- À la réception d'un message, il affiche sur la console "Préparez les articles suivants :" suivi de la liste des articles. Le programme affiche aussi s'il faut utiliser un emballage normal ou premium.

5. DSED_M07_TraitementCommande_CourrielsPremium (Consommateur)

- Il est abonné au sujet "commande.placee.premium".

- À la réception d'un message, il affiche sur la console "Commande premium" suivi du numéro de référence de la commande.
