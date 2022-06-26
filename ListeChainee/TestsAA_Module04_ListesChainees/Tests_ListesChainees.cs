using System;
using Xunit;
using AA_Module04_ListesChainees;
using System.Collections;
using System.Collections.Generic;

namespace TestsAA_Module04_ListesChainees
{
    public class Tests_ListesChainees
    {
        #region Constructeur/Propriétés
        [Fact]
        public void ConstructeurAucunParametre()
        {
            // Arrange
            int countAttendu = 0;

            // Act
            ListeChainee<int> listeChainee = new ListeChainee<int>();

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Null(listeChainee.PremierNoeud);
            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void ConstructeurPreconditionCollectionNull_Exception()
        {
            // Arrange
            List<int> collectionNulle = null;

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => { new ListeChainee<int>(collectionNulle); });
        }
        [Fact]
        public void ConstructeurCollectionVide()
        {
            // Arrange
            List<int> collectionVide = new List<int>();
            int countAttendu = 0;

            // Act
            ListeChainee<int> listeChainee = new ListeChainee<int>(collectionVide);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Null(listeChainee.PremierNoeud);
            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void ConstructeurCollectionUnElement()
        {
            // Arrange
            List<int> valeurs = new List<int>() { 999 };
            int valeurPremierNoeudAttendue = 999;
            int countAttendu = 1;

            // Act
            ListeChainee<int> listeChainee = new ListeChainee<int>(valeurs);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);

            Assert.NotNull(listeChainee.PremierNoeud);
            Assert.Equal(valeurPremierNoeudAttendue, listeChainee.PremierNoeud.Valeur);
            Assert.Null(listeChainee.PremierNoeud.Suivant);

            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void ConstructeurCollectionDeuxElements()
        {
            // Arrange
            List<int> valeurs = new List<int>() { 999, 777 };
            int valeurPremierNoeudAttendue = 999;
            int valeurDernierNoeudAttendue = 777;
            int countAttendu = 2;

            // Act
            ListeChainee<int> listeChainee = new ListeChainee<int>(valeurs);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);

            Assert.NotNull(listeChainee.PremierNoeud);
            Assert.NotNull(listeChainee.DernierNoeud);

            Assert.Equal(valeurPremierNoeudAttendue, listeChainee.PremierNoeud.Valeur);
            Assert.Equal(valeurDernierNoeudAttendue, listeChainee.DernierNoeud.Valeur);

            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
        }
        [Fact]
        public void ConstructeurCollectionPlusieursElements()
        {
            // Arrange
            List<int> valeurs = new List<int>() { 4, 3, 2, 1 };
            int valeurPremierNoeudAttendue = 4;
            int valeurDeuxiemeNoeudAttendue = 3;
            int valeurTroisiemeNoeudAttendue = 2;
            int valeurDernierNoeudAttendue = 1;
            int countAttendu = 4;

            // Act
            ListeChainee<int> listeChainee = new ListeChainee<int>(valeurs);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);

            Assert.NotNull(listeChainee.PremierNoeud);
            Assert.NotNull(listeChainee.DernierNoeud);

            Assert.Equal(valeurPremierNoeudAttendue, listeChainee.PremierNoeud.Valeur);

            Assert.Equal(valeurDeuxiemeNoeudAttendue, listeChainee.PremierNoeud.Suivant.Valeur);
            Assert.Equal(valeurTroisiemeNoeudAttendue, listeChainee.PremierNoeud.Suivant.Suivant.Valeur);

            Assert.Equal(valeurDernierNoeudAttendue, listeChainee.DernierNoeud.Valeur);

            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
        }

        [Fact]
        public void ThisIndexGet_PreconditionIndexNegatif_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });
            int indexPourRecherche = -1;
            int valeurObtenue = 0;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { valeurObtenue = listeChainee[indexPourRecherche]; });
        }
        [Fact]
        public void ThisIndexGet_PreconditionIndexEgalCount_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });
            int indexPourRecherche = 4;
            int valeurObtenue = 0;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { valeurObtenue = listeChainee[indexPourRecherche]; });
        }
        [Fact]
        public void ThisIndexGet_TrouverBonneValeur_ValeurAttendueEquals()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 10, 20, 30, 40 });
            int indexPourRecherche = 2;
            int valeurAttendue = 30;

            // Act
            int valeurObtenue = listeChainee[indexPourRecherche];

            // Assert
            Assert.Equal(valeurAttendue, valeurObtenue);
        }

        [Fact]
        public void ThisIndexSet_PreconditionIndexNegatif_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });
            int indexAModifier = -1;
            int nouvelleValeur = 999;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee[indexAModifier] = nouvelleValeur; });
        }
        [Fact]
        public void ThisIndexSet_PreconditionIndexEgalCount_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });
            int indexAModifier = 4;
            int nouvelleValeur = 999;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee[indexAModifier] = nouvelleValeur; });
        }
        [Fact]
        public void ThisIndexSet_ModifierValeur_ValeurAttendueEqual()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 10, 20, 30, 40 });
            int indexAModifier = 2;
            int nouvelleValeur = 999;

            // Act
            listeChainee[indexAModifier] = nouvelleValeur;

            // Assert
            Assert.Equal(nouvelleValeur, listeChainee[indexAModifier]);
        }
        #endregion

        #region Méthodes
        [Fact]
        public void Add_AucunElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>();
            int elementAjout = 1234;
            int countAttendu = 1;

            // Act
            listeChainee.Add(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.Null(listeChainee.PremierNoeud.Suivant);
            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void Add_UnElementDepart_AjouteAuDernierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999 });
            int elementAjout = 1234;
            int countAttendu = 2;

            // Act
            listeChainee.Add(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.DernierNoeud.Valeur);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.Same(listeChainee.PremierNoeud.Suivant, listeChainee.DernierNoeud);
        }
        [Fact]
        public void Add_DeuxElementDepart_ChangeLeDernierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 555 });
            int elementAjout = 1234;
            int countAttendu = 3;

            // Act
            listeChainee.Add(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.DernierNoeud.Valeur);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.Same(listeChainee.PremierNoeud.Suivant.Suivant, listeChainee.DernierNoeud);
        }

        [Fact]
        public void AddFirst_AucunElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>();
            int elementAjout = 1234;
            int countAttendu = 1;

            // Act
            listeChainee.AddFirst(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.Null(listeChainee.PremierNoeud.Suivant);
            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void AddFirst_UnElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999 });
            int elementAjout = 1234;
            int countAttendu = 2;

            // Act
            listeChainee.AddFirst(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.NotNull(listeChainee.DernierNoeud);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant);
        }
        [Fact]
        public void AddFirst_PlusieursElementsDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 888, 777, 666, 555 });
            int elementAjout = 1234;
            int countAttendu = 6;

            NoeudListeChainee<int> ancienPremierNoeud = listeChainee.PremierNoeud;

            // Act
            listeChainee.AddFirst(elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.NotSame(ancienPremierNoeud, listeChainee.PremierNoeud);
            Assert.Same(ancienPremierNoeud, listeChainee.PremierNoeud.Suivant);
        }

        [Fact]
        public void Clear_SupprimeNoeudsEtAjusteCount()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5 });
            int countAttendu = 0;

            int countAvantClear = listeChainee.Count;

            // Act
            listeChainee.Clear();

            // Assert
            Assert.NotEqual(countAvantClear, listeChainee.Count);
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Null(listeChainee.PremierNoeud);
            Assert.Null(listeChainee.DernierNoeud);
        }

        [Fact]
        public void Insert_PreconditionIndexNegatif_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2 });
            int valeurAjout = 999;
            int indexCible = -1;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.Insert(indexCible, valeurAjout); });
        }
        [Fact]
        public void Insert_PreconditionIndexPlusGrandQueCount_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2 });
            int valeurAjout = 999;
            int indexCible = 3;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.Insert(indexCible, valeurAjout); });
        }
        [Fact]
        public void Insert_AucunElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>();
            int elementAjout = 1234;
            int index = 0;
            int countAttendu = 1;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.Null(listeChainee.PremierNoeud.Suivant);
            Assert.Null(listeChainee.DernierNoeud);
        }
        [Fact]
        public void Insert_UnElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999 });
            int elementAjout = 1234;
            int index = 0;
            int countAttendu = 2;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.NotNull(listeChainee.DernierNoeud);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant);
        }
        [Fact]
        public void Insert_UnElementDepart_AjouteAuDernierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999 });
            int elementAjout = 1234;
            int index = 1;
            int countAttendu = 2;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.DernierNoeud.Valeur);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
            Assert.NotNull(listeChainee.PremierNoeud.Suivant);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant);
        }
        [Fact]
        public void Insert_DeuxElementDepart_AjouteAuPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 888 });
            int elementAjout = 1234;
            int index = 0;
            int countAttendu = 3;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Valeur);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant.Suivant);
        }
        [Fact]
        public void Insert_DeuxElementDepart_AjouteIndexUn()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 888 });
            int elementAjout = 1234;
            int index = 1;
            int countAttendu = 3;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Suivant.Valeur);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant.Suivant);
        }
        [Fact]
        public void Insert_DeuxElementDepart_AjouteAuDernierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 888 });
            int elementAjout = 1234;
            int index = 2;
            int countAttendu = 3;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.DernierNoeud.Valeur);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant.Suivant);
        }
        [Fact]
        public void Insert_PlusieursElementsDepartTest1_AjouteAuMillieu()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 999, 888, 777, 666, 555 });
            int elementAjout = 1234;
            int index = 2;
            int countAttendu = 6;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Suivant.Suivant.Valeur);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant.Suivant.Suivant.Suivant.Suivant);
        }
        [Fact]
        public void Insert_PlusieursElementsDepartTest2_AjouteAuMillieu()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int elementAjout = 1234;
            int index = 5;
            int countAttendu = 11;

            // Act
            listeChainee.Insert(index, elementAjout);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(elementAjout, listeChainee.PremierNoeud.Suivant.Suivant.Suivant.Suivant.Suivant.Valeur);
            Assert.Same(listeChainee.DernierNoeud, listeChainee.PremierNoeud.Suivant.Suivant.Suivant.Suivant.Suivant.Suivant.Suivant.Suivant.Suivant.Suivant);
        }

        [Fact]
        public void IndexOf_AucunElementDepart_RetourneMoinsUn()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { });
            int indexAttendu = -1;
            int valeurAChercher = 999;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementPresentUneFois_RetourneIndexCinq()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int indexAttendu = 5;
            int valeurAChercher = 6;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementPresentUneFois_RetourneIndexNeuf()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int indexAttendu = 9;
            int valeurAChercher = 10;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementPresentPlusieursFois_RetournePremierIndexOccurence()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            int indexAttendu = 0;
            int valeurAChercher = 1;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementNonPresent_RetourneMoinsUn()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            int indexAttendu = -1;
            int valeurAChercher = 2;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementNullPresentUneFois_RetourneIndexDeux()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", null, "d", "e" });
            int indexAttendu = 2;
            string valeurAChercher = null;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementNullPresentPlusieursFois_RetournePremierIndexOccurence()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", "c", "d", "e", null, null, "z" });
            int indexAttendu = 5;
            string valeurAChercher = null;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }
        [Fact]
        public void IndexOf_ElementNullNonPresent_RetourneMoinsUn()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", "c", "d", "e" });
            int indexAttendu = -1;
            string valeurAChercher = null;

            // Act
            int indexObtenu = listeChainee.IndexOf(valeurAChercher);

            // Assert
            Assert.Equal(indexAttendu, indexObtenu);
        }

        [Fact]
        public void Contains_AucunElementDepart_RetourneFalse()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { });
            int valeurAChercher = 999;

            // Act
            bool resultatObtenu = listeChainee.Contains(valeurAChercher);

            // Assert
            Assert.False(resultatObtenu);
        }
        [Fact]
        public void Contains_PlusieursElementsDepartElementRecherchePresent_RetourneTrue()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87 });
            int valeurAChercher = 87;

            // Act
            bool resultatObtenu = listeChainee.Contains(valeurAChercher);

            // Assert
            Assert.True(resultatObtenu);
        }
        [Fact]
        public void Contains_PlusieursElementsDepartElementRechercheNonPresent_RetourneFalse()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87 });
            int valeurAChercher = 64;

            // Act
            bool resultatObtenu = listeChainee.Contains(valeurAChercher);

            // Assert
            Assert.False(resultatObtenu);
        }
        [Fact]
        public void Contains_PlusieursElementsDepartElementNullRecherchePresent_RetourneTrue()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", "c", "d", "e", null, null, "z" });
            string valeurAChercher = null;

            // Act
            bool resultatObtenu = listeChainee.Contains(valeurAChercher);

            // Assert
            Assert.True(resultatObtenu);
        }
        [Fact]
        public void Contains_PlusieursElementsDepartElementNullRechercheNonPresent_RetourneFalse()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", "c", "d", "e", "z" });
            string valeurAChercher = null;

            // Act
            bool resultatObtenu = listeChainee.Contains(valeurAChercher);

            // Assert
            Assert.False(resultatObtenu);
        }

        [Fact]
        public void RemoveAt_PreconditionIndexNegatif_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87 });
            int indexASupprimer = 87;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.RemoveAt(indexASupprimer); });
        }
        [Fact]
        public void RemoveAt_PreconditionIndexEgalCount_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87 });
            int indexASupprimer = 87;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.RemoveAt(indexASupprimer); });
        }
        [Fact]
        public void RemoveAt_SupprimerPremierNoeud_NouveauPremierNoeud()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87 });
            int indexASupprimer = 0;
            int countAttendu = 5;

            int valeurPremierNoeudAttendu = 65;

            // Act
            listeChainee.RemoveAt(indexASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(valeurPremierNoeudAttendu, listeChainee.PremierNoeud.Valeur);
        }
        [Fact]
        public void RemoveAt_SupprimerDernierNoeud_AvantDernierNoeudDeviensDernier()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 3, 65, 32, 24, 65, 87, 1, 1, 1, 65, 9999 });
            int indexASupprimer = 10;
            int countAttendu = 10;

            int valeurDernierNoeudAttendu = 65;

            // Act
            listeChainee.RemoveAt(indexASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(valeurDernierNoeudAttendu, listeChainee.DernierNoeud.Valeur);
            Assert.Null(listeChainee.DernierNoeud.Suivant);
        }
        [Fact]
        public void RemoveAt_SupprimerNoeudMillieuChaine_ChangeSuivantDuNoeudPrecedent()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
            int indexASupprimer = 5;
            int countAttendu = 12;

            // Act
            listeChainee.RemoveAt(indexASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.Equal(5, listeChainee.PremierNoeud.Suivant.Suivant.Suivant.Suivant.Valeur);
            Assert.Equal(7, listeChainee.PremierNoeud.Suivant.Suivant.Suivant.Suivant.Suivant.Valeur);
        }

        [Fact]
        public void Remove_ElementNonPresent_RetourneFalse()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int countAttendu = 10;
            int valeurASupprimer = 0;

            // Act
            bool asEteSupprimer = listeChainee.Remove(valeurASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.False(asEteSupprimer);
        }
        [Fact]
        public void Remove_ElementPresent_RetourneTrue()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int countAttendu = 9;
            int valeurASupprimer = 6;

            // Act
            bool asEteSupprimer = listeChainee.Remove(valeurASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.True(asEteSupprimer);
        }
        [Fact]
        public void Remove_ElementNullPresent_RetourneTrue()
        {
            // Arrange
            ListeChainee<string> listeChainee = new ListeChainee<string>(new string[] { "a", "b", null, "c", "d", "e" });
            int countAttendu = 5;
            string valeurASupprimer = null;

            // Act
            bool asEteSupprimer = listeChainee.Remove(valeurASupprimer);

            // Assert
            Assert.Equal(countAttendu, listeChainee.Count);
            Assert.True(asEteSupprimer);
        }

        [Fact]
        public void CopyTo_PreconditionTableauDestinationNull_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });

            int[] tableauDestination = null;
            int indexDebut = 0;

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => { listeChainee.CopyTo(tableauDestination, indexDebut); });
        }
        [Fact]
        public void CopyTo_PreconditionIndexNegatif_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });

            int[] tableauDestination = new int[10];
            int indexDebut = -1;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.CopyTo(tableauDestination, indexDebut); });
        }
        [Fact]
        public void CopyTo_PreconditionIndexEgalLongueur_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });

            int[] tableauDestination = new int[10];
            int indexDebut = 10;

            // Act && Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { listeChainee.CopyTo(tableauDestination, indexDebut); });
        }
        [Fact]
        public void CopyTo_PreconditionPasAssezPlaceDansTableauDestination_Exception()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });

            int[] tableauDestination = new int[3];
            int indexDebut = 0;

            // Act && Assert
            Assert.Throws<ArgumentException>(() => { listeChainee.CopyTo(tableauDestination, indexDebut); });
        }
        [Fact]
        public void CopyTo_CopieAuDebutDuTableauDestination_TableauAttenduEqual()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            int[] tableauDestination = new int[20] { 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999 };
            int indexDebut = 0;

            int[] tableauAttendu = new int[20] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999 };

            // Act
            listeChainee.CopyTo(tableauDestination, indexDebut);

            // Assert
            Assert.Equal(tableauAttendu, tableauDestination);
        }
        [Fact]
        public void CopyTo_CopieAuMillieuDuTableauDestination_TableauAttenduEqual()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            int[] tableauDestination = new int[20] { 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999 };
            int indexDebut = 4;

            int[] tableauAttendu = new int[20] { 999, 999, 999, 999, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 999, 999, 999, 999, 999, 999 };

            // Act
            listeChainee.CopyTo(tableauDestination, indexDebut);

            // Assert
            Assert.Equal(tableauAttendu, tableauDestination);
        }
        [Fact]
        public void CopyTo_CopieALaFinDuTableauDestination_TableauAttenduEqual()
        {
            // Arrange
            ListeChainee<int> listeChainee = new ListeChainee<int>(new int[] { 1, 2, 3, 4 });

            int[] tableauDestination = new int[6] { 999, 999, 0, 0, 0, 0 };
            int indexDebut = 2;

            int[] tableauAttendu = new int[6] { 999, 999, 1, 2, 3, 4 };

            // Act
            listeChainee.CopyTo(tableauDestination, indexDebut);

            // Assert
            Assert.Equal(tableauAttendu, tableauDestination);
        }
        #endregion
    }
}
