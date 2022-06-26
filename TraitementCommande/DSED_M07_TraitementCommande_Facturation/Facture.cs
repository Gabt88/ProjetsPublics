using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSED_M07_TraitementCommande_Producteur;

namespace DSED_M07_TraitementCommande_Facturation
{
    internal class Facture
    {
        public decimal TotalSansTaxes { get; set; }
        public decimal TotalAvecTaxes { get; set; }
        public List<Article> Articles { get; set; }

        public Facture(decimal p_totalSansTaxes, decimal p_totalAvecTaxes, List<Article> p_articles)
        {
            if (p_totalSansTaxes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(p_totalSansTaxes));
            }

            if (p_totalAvecTaxes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(p_totalAvecTaxes));
            }

            if (p_articles is null)
            {
                throw new ArgumentNullException(nameof(p_articles));
            }

            this.TotalSansTaxes = p_totalSansTaxes;
            this.TotalAvecTaxes = p_totalAvecTaxes;
            this.Articles = new List<Article>();

            foreach (Article article in p_articles)
            {
                this.Articles.Add(article);
            }
        }
    }
}
