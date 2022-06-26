using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_Producteur
{
    public class Commande
    {
        public Guid Reference { get; set; }
        public string NomClient { get; set; }
        public List<Article> Articles { get; set; }

        public Commande()
        {
            ;
        }
        public Commande(Guid p_reference, string p_nomClient, List<Article> p_articles)
        {
            if (p_nomClient is null)
            {
                throw new ArgumentNullException(nameof(p_nomClient));
            }

            if (p_articles is null)
            {
                throw new ArgumentNullException(nameof(p_articles));
            }

            this.Reference = p_reference;
            this.NomClient = p_nomClient;
            this.Articles = new List<Article>();

            foreach (Article article in p_articles)
            {
                this.Articles.Add(article);
            }
        }

        public static Commande GenererCommandeAleatoire()
        {
            string[] prenomsPossibles = { "Gabriel", "David", "Kristopher", "Alexandre", "Philippe" };
            string[] nomsPossibles = { "Tremblay", "Trudel", "Parent", "Maltais", "Garcia" };
            Random rnd = new Random();

            Guid reference = Guid.NewGuid();
            string nomClient = prenomsPossibles[rnd.Next(prenomsPossibles.Length)] + " " + nomsPossibles[rnd.Next(nomsPossibles.Length)];

            int nombreArticles = rnd.Next(1, 6);
            List<Article> articles = new List<Article>(nombreArticles);

            for (int i = 0; i < articles.Capacity; i++)
            {
                articles.Add(Article.GenererArticleAleatoire());
            }

            return new Commande(reference, nomClient, articles);
        }
    }
}
