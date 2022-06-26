using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_Producteur
{
    public class Article
    {
        public Guid Reference { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }

        public Article()
        {
            ;
        }
        public Article(Guid p_reference, string p_nom, decimal p_prix, int p_quantite)
        {
            if (p_nom is null)
            {
                throw new ArgumentNullException(nameof(p_nom));
            }

            if (p_prix < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(p_prix));
            }

            if (p_quantite < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(p_quantite));
            }

            this.Reference = p_reference;
            this.Nom = p_nom;
            this.Prix = p_prix;
            this.Quantite = p_quantite;
        }

        public override string ToString()
        {
            return $"{this.Nom} ({this.Reference}) -> {this.Quantite} @ {this.Prix}$";
        }
        public static Article GenererArticleAleatoire()
        {
            string[] nomsPossibles = { "ordinateur", "divan", "matelas", "television", "refrigerateur" };
            decimal[] prixPossibles = { 199.99m, 399.99m, 599.99m, 799.99m };
            int[] quantitePossibles = { 1, 2, 3, 4 };
            Random rnd = new Random();

            Guid reference = Guid.NewGuid();
            string nom = nomsPossibles[rnd.Next(nomsPossibles.Length)];
            decimal prix = prixPossibles[rnd.Next(prixPossibles.Length)];
            int quantite = quantitePossibles[rnd.Next(quantitePossibles.Length)];

            return new Article(reference, nom, prix, quantite);
        }
    }
}
