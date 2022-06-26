using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA_Module04_ListesChainees
{
    public class ListeChainee<TypeElement> : IEnumerable<TypeElement>, IList<TypeElement>
    {
        // ***** PROPRIÉTÉS ***** //
        public NoeudListeChainee<TypeElement> PremierNoeud { get; private set; }

        public NoeudListeChainee<TypeElement> DernierNoeud { get; private set; }

        public TypeElement this[int index] 
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index", "L'index doit être plus grand ou égal à 0 et plus petit que le nombre d'éléments total.");
                }

                TypeElement valeurRecherchee = default;

                if (index == 0)
                {
                    valeurRecherchee = this.PremierNoeud.Valeur;
                }

                else if (index == this.Count - 1)
                {
                    valeurRecherchee = this.DernierNoeud.Valeur;
                }

                else
                {
                    NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;
                    int compteurIndex = 0;

                    while (compteurIndex <= index)
                    {
                        if (compteurIndex == index)
                        {
                            valeurRecherchee = noeudCourant.Valeur;
                        }

                        noeudCourant = noeudCourant.Suivant;
                        compteurIndex++;
                    }
                }
               
                return valeurRecherchee;
            }
            set 
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index", "L'index doit être plus grand ou égal à 0 et plus petit que le nombre d'éléments total.");
                }
               
                if (index == 0)
                {
                    this.PremierNoeud.Valeur = value;
                }

                else if (index == this.Count - 1)
                {
                    this.DernierNoeud.Valeur = value;
                }

                else
                {
                    NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;
                    int compteurIndex = 0;

                    while (compteurIndex <= index)
                    {
                        if (compteurIndex == index)
                        {
                            noeudCourant.Valeur = value;
                        }

                        noeudCourant = noeudCourant.Suivant;
                        compteurIndex++;
                    }
                }               
            } 
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        // ***** CONSTRUCTEURS ***** //
        public ListeChainee()
        {
            this.Clear();
        }
        public ListeChainee(IEnumerable<TypeElement> p_elements)
        {
            if (p_elements is null)
            {
                throw new ArgumentNullException("p_elements", "L'énumérable passé en paramètre ne peut pas être null.");
            }

            this.Clear();

            foreach (TypeElement valeur in p_elements)
            {
                this.Add(valeur);
            }
        }

        // ***** MÉTHODES ***** //
        public void Add(TypeElement item)
        {
            NoeudListeChainee<TypeElement> noeudAjout = new NoeudListeChainee<TypeElement>();
            noeudAjout.Valeur = item;
            noeudAjout.Suivant = null;

            if (this.PremierNoeud is null)
            {
                this.PremierNoeud = noeudAjout;
            }

            else
            {
                if (this.DernierNoeud is null)
                {
                    this.PremierNoeud.Suivant = noeudAjout;
                }

                else
                {
                    this.DernierNoeud.Suivant = noeudAjout;
                }

                this.DernierNoeud = noeudAjout;
            }

            this.Count++;
        }

        public void AddFirst(TypeElement item)
        {
            if (this.PremierNoeud is null && this.DernierNoeud is null)
            {
                this.Add(item);
            }

            else 
            {
                NoeudListeChainee<TypeElement> noeudAjout = new NoeudListeChainee<TypeElement>();
                noeudAjout.Valeur = item;
                noeudAjout.Suivant = this.PremierNoeud;

                if (this.DernierNoeud is null)
                {
                    this.DernierNoeud = this.PremierNoeud;
                }

                this.PremierNoeud = noeudAjout;
                this.Count++;
            }         
        }

        public void Clear()
        {
            this.PremierNoeud = null;
            this.DernierNoeud = null;
            this.Count = 0;
        }

        public bool Contains(TypeElement item)
        {
            bool listeChaineeContientItem = this.IndexOf(item) == -1 ? false : true;

            return listeChaineeContientItem;
        }

        public void CopyTo(TypeElement[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException("array", "Le tableau destination ne peut pas être null.");
            }

            if (arrayIndex < 0 || arrayIndex > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "L'index de début de copie doit être plus grand ou égal à 0 et plus petit que la longueur.");
            }

            if (this.Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("array", "Il n'y as pas assez d'espace dans le tableau pour copier les valeurs du tableau source à partir de l'index indiqué.");
            }

            NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;

            while (noeudCourant is not null)
            {
                array[arrayIndex] = noeudCourant.Valeur;
                arrayIndex++;
                noeudCourant = noeudCourant.Suivant;
            }
        }

        public int IndexOf(TypeElement item)
        {
            NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;
            Func<TypeElement, TypeElement, bool> comparaisonEgalite = null;
            int compteurIndex = 0;
            int positionIndex = -1;

            if (item is null)
            {
                comparaisonEgalite = (valeurNoeud, item) => valeurNoeud is null;
            }

            else
            {
                comparaisonEgalite = (valeurNoeud, item) => valeurNoeud.Equals(item);
            }
          
            while (noeudCourant is not null && positionIndex == -1)
            {
                if (comparaisonEgalite(noeudCourant.Valeur, item) == true)
                {
                    positionIndex = compteurIndex;
                }

                noeudCourant = noeudCourant.Suivant;
                compteurIndex++;
            }

            return positionIndex;
        }

        public void Insert(int index, TypeElement item)
        {
            if (index < 0 || index > this.Count)
            {
                throw new ArgumentOutOfRangeException("index", "L'index doit être plus grand ou égal à 0 et plus petit ou égal au nombre d'éléments total.");
            }

            if (index == 0)
            {
                this.AddFirst(item);
            }

            else if (index == this.Count)
            {
                this.Add(item);
            }

            else
            {
                NoeudListeChainee<TypeElement> noeudAjout = new NoeudListeChainee<TypeElement>();
                noeudAjout.Valeur = item;
                noeudAjout.Suivant = null;

                NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;
                int compteurIndex = 0;

                while (compteurIndex < index)
                {
                    if (compteurIndex == index - 1)
                    {
                        noeudAjout.Suivant = noeudCourant.Suivant;
                        noeudCourant.Suivant = noeudAjout;
                        noeudCourant = noeudAjout;
                    }

                    noeudCourant = noeudCourant.Suivant;
                    compteurIndex++;
                }

                this.Count++;
            }
        }

        public bool Remove(TypeElement item)
        {
            bool asEteSupprimer = false;
            int indexPremiereOccurence = this.IndexOf(item);
            
            if (indexPremiereOccurence != -1)
            {
                this.RemoveAt(indexPremiereOccurence);
                asEteSupprimer = true;
            }

            return asEteSupprimer;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException("index", "L'index doit être plus grand ou égal à 0 et plus petit que le nombre d'éléments total.");
            }
           
            if (index == 0)
            {
                this.PremierNoeud = this.PremierNoeud.Suivant;
            }

            else
            {
                NoeudListeChainee<TypeElement> noeudCourant = this.PremierNoeud;
                int compteurIndex = 0;

                while (compteurIndex < index)
                {
                    if (compteurIndex == index - 1)
                    {
                        noeudCourant.Suivant = noeudCourant.Suivant.Suivant;

                        if (index == this.Count - 1)
                        {
                            this.DernierNoeud = noeudCourant;
                        }
                    }
                     
                    noeudCourant = noeudCourant.Suivant;
                    compteurIndex++;
                }
            }

            this.Count--;
        }

        public IEnumerator<TypeElement> GetEnumerator()
        {
            return new EnumeratorListeChainee<TypeElement>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
