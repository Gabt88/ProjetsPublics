ALTER SESSION SET NLS_DATE_FORMAT = 'yyyy-mm-dd';
/*************************************************
  CR�ATION DES TABLES
**************************************************/
CREATE TABLE RM_Films
(
  "id"        		number(7) NOT NULL,
  titre             varchar2(100) NOT NULL,
  dateParution      date NOT NULL,
  "description"     varchar2(500),
  synopsis			varchar2(2000) NOT NULL,
  budget			number(10,2),
  profits			number(11,2),
  pointage			number(2),
  dateModification	date,
  splashImage		varchar2(50) NOT NULL,
  nbVisitesPage		number(6),
  
  CONSTRAINT PK_Films_IdFilm PRIMARY KEY ("id"),
  CONSTRAINT CK_Films_PointageEntre0Et10 CHECK (pointage BETWEEN 0 AND 10),
  CONSTRAINT CK_Films_BudgetProfitPositif CHECK (budget >= 0 AND profits >= 0),
  CONSTRAINT CK_Films_DateModifPlusGrandeOuEgaleDateParution CHECK (dateModification >= dateParution)
);

ALTER TABLE RM_Films
ADD LangueOriginale char(2) DEFAULT 'FR';

CREATE TABLE RM_Membres
(
  -- Cr�ation sans les contraintes, doit faire un ALTER par la suite.
  "id"      	number(7) NOT NULL,
  login    		varchar(50) NOT NULL,
  motPasse    	varchar2(100) NOT NULL,
  email         varchar2(100),
  prenom        varchar2(50) NOT NULL,
  nom           varchar2(50) NOT NULL,
  idPays      	number(7) DEFAULT 1 NOT NULL,
  dateNaissance	date NOT NULL,
  sexe			char(1) NOT NULL,
  siteWeb		varchar2(200) 
);

CREATE TABLE RM_Pays
(
  "id"	number(7) NOT NULL,
  nom   varchar2(50) NOT NULL,
  identifiantPays char(2) NOT NULL,
  
  CONSTRAINT PK_Pays_IdPays PRIMARY KEY ("id"),
  CONSTRAINT CK_Pays_IdentifiantPays2Lettres CHECK (REGEXP_LIKE (identifiantPays,'^[a-zA-Z]{2}$'))
);

ALTER TABLE RM_Membres
ADD 
(
    CONSTRAINT PK_Membres_IdMembre PRIMARY KEY ("id"),
    CONSTRAINT FK_Membres_IdPays FOREIGN KEY (idPays) REFERENCES RM_Pays("id"),
    CONSTRAINT CK_Membres_Sexe1Lettre CHECK (REGEXP_LIKE (sexe,'^[a-zA-Z]$')),
    CONSTRAINT CK_Membres_FormatEmail CHECK (email LIKE '_%@_%.__%'),
    CONSTRAINT CK_Membres_FormatSiteWeb CHECK (siteWeb LIKE 'http://_____%' OR siteWeb LIKE 'https://_____%')
);

CREATE TABLE RM_Genres
(
  "id" 		number(7) NOT NULL,
  nom	 	varchar2(40) NOT NULL,
  
  CONSTRAINT PK_Genres_IdGenre PRIMARY KEY ("id")
);

CREATE TABLE RM_ChangementsEnAttente_Films
(
  "id"        			number(7) NOT NULL,
  idFilm       			number(7) NOT NULL,
  titre             	varchar2(100) NOT NULL,
  dateParution      	date NOT NULL,
  "description"       	varchar2(500),
  synopsis				varchar2(2000) NOT NULL,
  budget				number(10,2),
  profits				number(11,2),
  pointage				number(2),
  splashImage			varchar2(50) NOT NULL,
  dateAjout				date DEFAULT SYSDATE NOT NULL,
  idMembreModification	number(7) NOT NULL,
  actif					char(1) NOT NULL,
  
  CONSTRAINT PK_ChangementsEnAttenteFilms_IdChangement PRIMARY KEY ("id"),
  CONSTRAINT FK_ChangementsEnAttenteFilms_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id"),
  CONSTRAINT FK_ChangementsEnAttenteFilms_IdMembreModif FOREIGN KEY (idMembreModification) REFERENCES RM_Membres("id"),
  CONSTRAINT CK_ChangementsEnAttenteFilms_PointageEntre0Et10 CHECK (pointage BETWEEN 0 AND 10),
  CONSTRAINT CK_ChangementsEnAttenteFilms_Actif0Ou1 CHECK (actif = '0' OR actif = '1'),
  CONSTRAINT CK_ChangementsEnAttenteFilms_BudgetProfitPositif CHECK (budget >= 0 AND profits >= 0),
  CONSTRAINT CK_ChangementsEnAttenteFilms_DateAjoutPlusGrandeOuEgaleDateParution CHECK (dateAjout >= dateParution)
);

-- Table qu'on as cr�e de A � Z
CREATE TABLE RM_Messages_MurFilms
(
    "id"        		number(7) NOT NULL,
    idMembre        	number(7) NOT NULL,
    idFilm        	    number(7) NOT NULL,
    message        	    varchar2(200) NULL,
    dateAjout        	DATE DEFAULT SYSDATE NULL,
    
    CONSTRAINT PK_MessagesMurFilms_IdMessage PRIMARY KEY ("id"),
    CONSTRAINT FK_MessagesMurFilms_IdMembre FOREIGN KEY (idMembre) REFERENCES RM_Membres("id"),
    CONSTRAINT FK_MessagesMurFilms_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id")
);

CREATE TABLE RM_Votes_Films_EnAttente
(
  idMembre 		number(7) NOT NULL,
  idAttente 	number(7) NOT NULL,
  pourContre	char(1) NOT NULL,
  
  CONSTRAINT PK_VotesFilmsAttente_IdMembreIdAttente PRIMARY KEY (idMembre,idAttente),
  CONSTRAINT FK_VotesFilmsAttente_IdMembre FOREIGN KEY (idMembre) REFERENCES RM_Membres("id"),
  CONSTRAINT FK_VotesFilmsAttente_IdAttente FOREIGN KEY (idAttente) REFERENCES RM_ChangementsEnAttente_Films("id"),
  CONSTRAINT CK_VotesFilmsAttente_PourContre0Ou1 CHECK (pourContre = '0' OR pourContre = '1')
);

CREATE TABLE RM_Votes_MurFilms
(
  idMembre 		number(7) NOT NULL,
  idMessage 	number(7) NOT NULL,
  pourContre	char(1) NOT NULL,
  
  CONSTRAINT PK_VotesMurFilms_IdMembreIdMessage PRIMARY KEY (idMembre,idMessage),
  CONSTRAINT FK_VotesMurFilms_IdMembre FOREIGN KEY (idMembre) REFERENCES RM_Membres("id"),
  CONSTRAINT FK_VotesMurFilms_IdMessage FOREIGN KEY (idMessage) REFERENCES RM_Messages_MurFilms("id"),
  CONSTRAINT CK_VotesMurFilms_PourContre0Ou1 CHECK (pourContre = '0' OR pourContre = '1')
);

CREATE TABLE RM_FilmsFavoris_Membres
(
  idMembre 		number(7) NOT NULL,
  idFilm	 	number(7) NOT NULL,
  
  CONSTRAINT PK_FilmsFavorisMembres_IdMembreIdFilm PRIMARY KEY (idMembre,idFilm),
  CONSTRAINT FK_FilmsFavorisMembres_IdMembre FOREIGN KEY (idMembre) REFERENCES RM_Membres("id"),
  CONSTRAINT FK_FilmsFavorisMembres_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id")
);

CREATE TABLE RM_GenresFilms
(
  idFilm 		number(7) NOT NULL,
  idGenre	 	number(7) NOT NULL,
  
  CONSTRAINT PK_GenresFilms_IdFilmIdGenre PRIMARY KEY (idFilm,idGenre),
  CONSTRAINT FK_GenresFilms_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id"),
  CONSTRAINT FK_GenresFilms_IdGenre FOREIGN KEY (idGenre) REFERENCES RM_Genres("id")
);

CREATE TABLE RM_Roles
(
  "id" 		number(7) NOT NULL,
  nom	 	varchar2(40) NOT NULL,
  
  CONSTRAINT PK_Roles_IdRole PRIMARY KEY ("id")
);

CREATE TABLE RM_Personnes
(
  "id" 			number(7) NOT NULL,
  prenom        varchar2(50) NOT NULL,
  nom           varchar2(50) NOT NULL,
  dateNaissance	date NOT NULL,
  sexe			char(1) NOT NULL,
  
  CONSTRAINT PK_Personnes_IdPersonne PRIMARY KEY ("id"),
  CONSTRAINT CK_Personnes_Sexe1Lettre CHECK (sexe = 'F' OR sexe = 'H')
);

CREATE TABLE RM_Employes
(
  idRole 		number(7) NOT NULL,
  idPersonne 	number(7) NOT NULL,
  idFilm	 	number(7) NOT NULL,
  
  CONSTRAINT PK_Employes_IdRoleIdPersonneIdFilm PRIMARY KEY (idRole,idPersonne,IdFilm),
  CONSTRAINT FK_Employes_IdRole FOREIGN KEY (idRole) REFERENCES RM_Roles("id"),
  CONSTRAINT FK_Employes_IdPersonne FOREIGN KEY (idPersonne) REFERENCES RM_Personnes("id"),
  CONSTRAINT FK_Employes_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id")
);

-- Table qu'on as cr�e de A � Z
CREATE TABLE RM_Critiques
(
  "id" 		            number(7) NOT NULL,
  pointageGlobal 	    AS (ROUND((pointageScenario+pointageDistribution+pointageSFX)/3,2)),
  pointageScenario      number(4,2) NOT NULL,
  pointageDistribution  number(4,2) NOT NULL,
  pointageSFX           number(4,2) NOT NULL,
  commentaires          varchar2(200) NULL,
  dateAjout        	    DATE DEFAULT SYSDATE NULL,
  idMembre        	    number(7) NOT NULL,
  idFilm        	    number(7) NOT NULL,
  
  CONSTRAINT PK_Critiques_IdCritique PRIMARY KEY ("id"),
  CONSTRAINT FK_Critiques_IdMembre FOREIGN KEY (idMembre) REFERENCES RM_Membres("id"),
  CONSTRAINT FK_Critiques_IdFilm FOREIGN KEY (idFilm) REFERENCES RM_Films("id"),
  CONSTRAINT CK_Critiques_Pointages CHECK ((pointageScenario BETWEEN 0 AND 10) AND (pointageDistribution BETWEEN 0 AND 10) AND (pointageSFX BETWEEN 0 AND 10))
);



/*************************************************
  CR�ATION DES INDEX
**************************************************/
CREATE INDEX IDX_Membres_IdPays ON RM_Membres(idPays);
CREATE INDEX IDX_Changements_IdFilmIdMembreModif ON RM_ChangementsEnAttente_Films (idFilm,idMembreModification);
CREATE INDEX IDX__MessagesMurFilms_IdFilmIdMembre ON RM_Messages_MurFilms (idFilm,idMembre);
CREATE INDEX IDX_Critiques_IdFilmIdMembre ON RM_Critiques (idFilm,idMembre);
CREATE INDEX IDX_Films_DateParution ON RM_Films (dateParution);
CREATE INDEX IDX_Films_Pointage ON RM_Films (pointage);
CREATE INDEX IDX_Changements_DateAjout ON RM_ChangementsEnAttente_Films (dateAjout);
CREATE INDEX IDX_Films_ProfitsBudget ON RM_Films (profits,budget);

/*************************************************
  CR�ATION DES S�QUENCES
**************************************************/
CREATE SEQUENCE seq_idMembre START WITH 1;
CREATE SEQUENCE seq_idCritique START WITH 1;

/*************************************************
  CR�ATION DES COMMENTAIRES
**************************************************/
COMMENT ON TABLE RM_Films IS 'Liste des films avec leurs informations.';
COMMENT ON TABLE RM_Genres IS 'Liste des genres de films possibles.';
COMMENT ON TABLE RM_Membres IS 'Liste des membres avec leurs informations.';
COMMENT ON TABLE RM_Pays IS 'Liste des pays.';
COMMENT ON TABLE RM_Votes_MurFilms IS 'Table interm�diaire qui permet de lier le vote (pour ou contre) d''un membre � un ou des messages.';
COMMENT ON TABLE RM_Messages_MurFilms IS 'Liste des messages r�dig�s par les membres faisant r�f�rence � un film en particulier.';
COMMENT ON TABLE RM_GenresFilms IS 'Table interm�diaire qui permet de lier un film � plusieurs genres et vice-versa.';
COMMENT ON TABLE RM_FilmsFavoris_Membres IS 'Table interm�diaire qui permet de lier les membres avec leur films favoris.';
COMMENT ON TABLE RM_Votes_Films_EnAttente IS 'Table interm�diaire qui lie les membres aux votes des changements des films en attente.';
COMMENT ON TABLE RM_ChangementsEnAttente_Films IS 'Liste des changements des films en attente.';
COMMENT ON TABLE RM_Critiques IS 'Liste des critiques des films avec les membres correspondants.';
COMMENT ON TABLE RM_Personnes IS 'Liste des personnes ayant particip�s aux films avec leurs informations.';
COMMENT ON TABLE RM_Employes IS 'Table interm�diaire qui lie une personne, un r�le, et un film.';
COMMENT ON TABLE RM_Roles IS 'Liste des r�les.';

COMMENT ON COLUMN RM_FILMS."id" IS 'Identifiant unique d''un film.';
COMMENT ON COLUMN RM_FILMS.titre IS 'Titre d''un film.';
COMMENT ON COLUMN RM_FILMS.dateParution IS 'Date que le film est sorti.';
COMMENT ON COLUMN RM_FILMS."description" IS 'Description des caract�ristique du film';
COMMENT ON COLUMN RM_FILMS.synopsis IS 'Bref r�sum� de l''histoire du film.';
COMMENT ON COLUMN RM_FILMS.budget IS 'Argent disponible pour la production du film.';
COMMENT ON COLUMN RM_FILMS.profits IS 'Argent engendr� par la production du film.';
COMMENT ON COLUMN RM_FILMS.pointage IS 'Score entre 0 et 10.';
COMMENT ON COLUMN RM_FILMS.dateModification IS 'Derni�re date qu''un changement a �t� fait sur les informations.';
COMMENT ON COLUMN RM_FILMS.splashImage IS 'Fichier contenant l''image couverture du film.';
COMMENT ON COLUMN RM_FILMS.nbVisitesPage IS 'Nombre de visite sur la page du film';

/*************************************************
  INSERTION DES DONN�ES
**************************************************/
-- � Compl�ter
INSERT INTO RM_Genres VALUES (1, 'Com�die');
INSERT INTO RM_Genres VALUES (2, 'Drame');
INSERT INTO RM_Genres VALUES (3, 'Horreur');
INSERT INTO RM_Genres VALUES (4, 'Musical');

INSERT INTO RM_Pays VALUES (1, 'Canada', 'CA');
INSERT INTO RM_Pays VALUES (2, 'Italie', 'IT');
INSERT INTO RM_Pays VALUES (3, 'France', 'FR');
INSERT INTO RM_Pays VALUES (4, 'Espagne', 'ES');
INSERT INTO RM_Pays VALUES (5, 'Mexique', 'ME');
INSERT INTO RM_Pays VALUES (6, '�tats-Unis', 'EU');

INSERT INTO RM_Roles VALUES (1, 'Acteur');
INSERT INTO RM_Roles VALUES (2, 'Directeur');
INSERT INTO RM_Roles VALUES (3, 'Musicien');
INSERT INTO RM_Roles VALUES (4, 'Sc�nariste');
INSERT INTO RM_Roles VALUES (5, 'Figurant');
INSERT INTO RM_Roles VALUES (6, 'Maquilleur');
INSERT INTO RM_Roles VALUES (7, 'Cam�raman');

INSERT INTO RM_Personnes VALUES (1, 'Olivia', 'Colman', '1974-01-30', 'F');
INSERT INTO RM_Personnes VALUES (2, 'Anthony', 'Hopkins', '1937-12-31', 'H');
INSERT INTO RM_Personnes VALUES (3, 'Ryan', 'Gosling', '1980-11-12', 'H');
INSERT INTO RM_Personnes VALUES (4, 'Damien', 'Chazelle', '1985-01-19', 'H');

INSERT INTO RM_Films ("id", titre, dateParution, synopsis, splashImage) VALUES (1, 'The Father', '2021-03-19', 'A man refuses all assistance from his daughter as he ages. As he tries to make sense of his changing circumstances, he begins to doubt his loved ones, his own mind and even the fabric of his reality.', 'the-father.jpg');
INSERT INTO RM_Films ("id", titre, dateParution, synopsis, splashImage) VALUES (2, 'La La Land', '2016-12-25', 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future. ', 'la-la-land.jpg');
INSERT INTO RM_Films ("id", pointage, titre, dateParution, synopsis, splashImage) VALUES (3, 8, 'Film3', '2016-12-25', 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future. ', 'la-la-land.jpg');
INSERT INTO RM_Films ("id", titre, dateParution, synopsis, splashImage) VALUES (4, 'testRequete6', '2016-12-25', 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future. ', 'la-la-land.jpg');
INSERT INTO RM_Films ("id", pointage, titre, dateParution, synopsis, splashImage) VALUES (5, 8, 'testRequete6', '2016-12-25', 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future. ', 'la-la-land.jpg');

INSERT INTO RM_Employes VALUES (1,1,1);
INSERT INTO RM_Employes VALUES (1,2,1);
INSERT INTO RM_Employes VALUES (1,3,2);
INSERT INTO RM_Employes VALUES (2,4,2);
INSERT INTO RM_Employes VALUES (5,3,4);
INSERT INTO RM_Employes VALUES (5,4,5);

INSERT INTO RM_GenresFilms VALUES (1,2);
INSERT INTO RM_GenresFilms VALUES (2,1);
INSERT INTO RM_GenresFilms VALUES (2,4);
INSERT INTO RM_GenresFilms VALUES (3,2);
INSERT INTO RM_GenresFilms VALUES (4,1);
INSERT INTO RM_GenresFilms VALUES (5,1);

INSERT INTO RM_Membres("id", login, motPasse, email, prenom, nom, idPays, sexe, dateNaissance, siteWeb) VALUES (seq_idMembre.NEXTVAL, 'Mike07', 'soleil123', 'mike@csfoy.ca', 'Michael', 'Drouin', 1, 'M', '2000-01-10', 'http://www.mike.com');
INSERT INTO RM_Membres ("id", login, motPasse, email, prenom, nom, idPays, sexe, dateNaissance, siteWeb) VALUES (seq_idMembre.NEXTVAL, 'Boumso007', 'admin2', 'boumboum@csfoy.ca', 'Andr�', 'Boumso', 2, 'M', '1970-01-20', 'https://www.boum.com');

INSERT INTO RM_FilmsFavoris_Membres (idMembre, idFilm) VALUES (1, 2);
INSERT INTO RM_FilmsFavoris_Membres (idMembre, idFilm) VALUES (2, 1);

INSERT INTO RM_Messages_MurFilms ("id", idMembre, idFilm, message, dateAjout) VALUES (1, 2, 2, 'Excellent film pour les enfants, je recommande fortement', '2017-03-25');
INSERT INTO RM_Messages_MurFilms ("id", idMembre, idFilm, message, dateAjout) VALUES (2, 1, 1, 'Vraiment un film mauvais', '2021-04-22');

INSERT INTO RM_Votes_MurFilms (idMembre, idMessage, pourContre) VALUES (1, 2, '1');
INSERT INTO RM_Votes_MurFilms (idMembre, idMessage, pourContre) VALUES (2, 1, '0');

INSERT INTO RM_Critiques ("id", pointageScenario, pointageDistribution, pointageSFX, dateAjout, idMembre, idFilm) VALUES (seq_idCritique.NEXTVAL, 5.5, 4.6, 2.4, '2021-05-20', 1, 1);
INSERT INTO RM_Critiques ("id", pointageScenario, pointageDistribution, pointageSFX, dateAjout, idMembre, idFilm) VALUES (seq_idCritique.NEXTVAL, 2.5, 7.6, 3.4, '2017-07-18', 2, 2);

INSERT INTO RM_ChangementsEnAttente_Films ("id", idFilm, titre, dateParution, "description", synopsis, budget, profits, pointage, splashImage, dateAjout, idMembreModification, actif) VALUES (1, 1, 'The Father', '2021-03-19', 'voici la description', 'A man refuses all assistance from his daughter as he ages. As he tries to make sense of his changing circumstances, he begins to doubt his loved ones, his own mind and even the fabric of his reality.', 10000, 200000, 7, 'image.jpg', '2021-11-20', 1, '1');
INSERT INTO RM_ChangementsEnAttente_Films ("id", idFilm, titre, dateParution, "description", synopsis, budget, profits, pointage, splashImage, dateAjout, idMembreModification, actif) VALUES (2, 2, 'La La Land', '2016-12-25', 'la description du film', 'While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future. ', 100000, 1500000, 4, 'image2.jpg', '2018-02-02', 2, '0');

INSERT INTO RM_Votes_Films_EnAttente (idMembre, idAttente, pourContre) VALUES (1, 2, '0');
INSERT INTO RM_Votes_Films_EnAttente (idMembre, idAttente, pourContre) VALUES (2, 1, '1');

-- V�rification
-- SELECT * FROM RM_Films;
-- SELECT * FROM RM_Genres;
-- SELECT * FROM RM_Membres;
-- SELECT * FROM RM_Pays;
-- SELECT * FROM RM_Votes_MurFilms;
-- SELECT * FROM RM_Messages_MurFilms;
-- SELECT * FROM RM_GenresFilms;
-- SELECT * FROM RM_FilmsFavoris_Membres;
-- SELECT * FROM RM_Votes_Films_EnAttente;
-- SELECT * FROM RM_ChangementsEnAttente_Films;
-- SELECT * FROM RM_Critiques;
-- SELECT * FROM RM_Personnes;
-- SELECT * FROM RM_Employes;
-- SELECT * FROM RM_Roles;

/*************************************************
  REQU�TES
**************************************************/
-- 1. Combien y-a-t-il eu de films par genre au cours des 3 derniers mois ? (5pts)
SELECT g.nom AS "genre", COUNT(*) AS "nombre film" FROM RM_Films f
    INNER JOIN RM_GenresFilms gf
        ON f."id" = gf.idFilm
    INNER JOIN RM_Genres g
        ON gf.idGenre = g."id"
    WHERE f.dateParution >= ADD_MONTHS(SYSDATE, -3)
    GROUP BY g.nom;
    
-- #2 Quels sont les films, en ordre de popularit�, dans le genre Drame�? (5pts)
SELECT
    F.titre
    ,F.pointage
FROM RM_Films F
INNER JOIN RM_GenresFilms GF
    ON F."id" = GF.idFilm
WHERE GF.idGenre = (SELECT "id" FROM RM_Genres WHERE LOWER(nom) = 'drame')   
ORDER BY F.pointage ASC; 
    
-- #3. Quels films n'ont pas �t� critiqu�s au cours des derniers 6 mois ? (5pts)
SELECT UNIQUE titre FROM RM_Films 
    WHERE "id" NOT IN (SELECT idFilm FROM RM_Critiques WHERE dateAjout >= ADD_MONTHS(SYSDATE, -6));

-- #4 Quelle est la liste des films dont les profits sont inf�rieurs � 3 fois le budget ? (3pts)
SELECT * FROM RM_Films
WHERE profits < (budget * 3);

-- #5 : Cr�ez une vue qui listera les films qui ont �t� modifi�s 2 fois. 
-- Pour qu�une modification soit valide, elle doit avoir obtenu au moins 10 votes ��Pour�� (Thumbs up). (6pts)
CREATE VIEW FilmsModifieDeuxFois AS
SELECT 
    titre
FROM RM_ChangementsEnAttente_Films
WHERE "id" IN 
(
    SELECT 
        idAttente
    FROM RM_ChangementsEnAttente_Films C
    INNER JOIN RM_Votes_Films_EnAttente VFA
        ON C."id" = VFA.idAttente
    WHERE VFA.pourContre = '1'
    GROUP BY VFA.idAttente
    HAVING COUNT(VFA.idAttente) >= 10
)    
GROUP BY titre
HAVING COUNT("id") >= 2;

SELECT * FROM FilmsModifieDeuxFois;

-- #6 Listez les personnes qui ont particip� � au moins 2 com�dies dans la derni�re ann�e. (5pts)
SELECT (P.prenom || ' ' || P.nom) As "Nom complet" FROM RM_Personnes P
INNER JOIN RM_Employes E
    ON P."id" = E.idPersonne
INNER JOIN RM_Films F
    ON E.idFilm = F."id"
INNER JOIN RM_GenresFilms G
    ON F."id" = G.idFilm
WHERE 
(F.dateParution >= ADD_MONTHS(F.dateParution, -12))
AND
(G.idGenre = (SELECT "id" FROM RM_Genres WHERE LOWER(nom) = 'com�die'))
GROUP BY P.prenom, P.nom
HAVING COUNT(F."id") >= 2;

-- 7. Affichez le total des profits, par genre, pour la derni�re ann�e. Les montants doivent �tre affich�s apr�s taxes et avec 2 chiffres apr�s la virgule et un signe de dollar.
SELECT g.nom AS "genre", (TO_CHAR(ROUND(SUM(f.profits - (f.profits * 0.15)), 2), '9,999,999.99') || '$') AS "profit apr�s taxes" FROM RM_Genres g
    INNER JOIN RM_GenresFilms gf
        ON g."id" = gf.idGenre
    INNER JOIN RM_Films f
        ON gf.idFilm = f."id"
    WHERE dateParution >= ADD_MONTHS(SYSDATE, -12)
    GROUP BY g.nom;
    
-- #8 : Quels sont les 10 meilleurs films de l�ann�e (films revenant le plus souvent dans les favoris des membres)�? (5pts)
SELECT
    F.titre,
    COUNT(*) AS "Nb fois favori"
FROM RM_Films F 
INNER JOIN RM_FilmsFavoris_Membres FFM
    ON F."id" = FFM.idFilm
WHERE (dateParution >= ADD_MONTHS(dateParution, -12))
GROUP BY F.titre
ORDER BY COUNT(*) DESC
FETCH FIRST 10 ROWS ONLY;

-- 9. Affichez la liste des acteurs avec le nombre de films dans lesquels ils ont jou�e en tant qu'acteur
SELECT p.prenom || ' ' || p.nom AS "Acteur", COUNT(*) AS "Nombre de film" FROM RM_Personnes p
    INNER JOIN RM_Employes e
        ON p."id" = e.idPersonne
    WHERE e.idRole = (SELECT "id" FROM RM_Roles WHERE LOWER(nom) = 'acteur')
    GROUP BY p.prenom, p.nom; 
    
-- #10 : Quel genre a le plus de films�? Affichez seulement UN r�sultat. (5pts)
SELECT
    G.nom,
    COUNT(GF.idFilm) AS "Nb films"
FROM RM_Genres G
INNER JOIN RM_GenresFilms GF
    ON G."id" = GF.idGenre
GROUP BY G.nom
ORDER BY COUNT(GF.idFilm) DESC
FETCH FIRST 1 ROWS ONLY;

-- 11. Quel pourcentage des membres vient du canada
SELECT UNIQUE (ROUND((SELECT COUNT(*) FROM RM_Membres WHERE idPays = 1) * 100 / (SELECT COUNT(*) FROM RM_Membres), 2) || '%') AS "Pourcentage membre canadien" FROM RM_Membres;

-- #12 : Listez les membres dont la 3i�me lettre du courriel est un A et qui habitent au Canada, en France ou en Belgique. 
-- Affichez les noms selon le format suivant�: ��Filiatreault, K.��. 
-- Assurez-vous que les premi�res lettres du nom de famille et du pr�nom soient en majuscules et que les autres lettres soient en minuscules. 
-- Remarquez aussi la virgule, l�espace et le point. (5pts)
SELECT 
    (INITCAP(nom) || ', ' || UPPER(SUBSTR(prenom,1,1)) || '.') AS "NOM",
    email
FROM RM_Membres
WHERE 
(idPays IN (SELECT "id" FROM RM_Pays WHERE LOWER(nom) IN ('canada', 'france', 'belgique')))
AND
(email LIKE '__a%');

-- #13 : Listez les personnes ayant une autre personne comme ��jumeau��. 
-- On d�termine que deux personnes sont jumelles s�ils ont la m�me ann�e et le m�me mois de naissance. (5pts)
SELECT
    LISTAGG((prenom || ' ' || nom), ', ') WITHIN GROUP (ORDER BY prenom) AS "Jumeaux"
FROM RM_Membres
GROUP BY TO_CHAR(dateNaissance,'YY-MM')
HAVING COUNT(TO_CHAR(dateNaissance,'YY-MM')) > 1; 