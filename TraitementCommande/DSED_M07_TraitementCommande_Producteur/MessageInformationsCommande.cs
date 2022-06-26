using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_Producteur
{
    public class MessageInformationsCommande
    {
        public string Sujet { get; set; }
        public Commande Commande { get; set; }

        public MessageInformationsCommande()
        {
            ;
        }
        public MessageInformationsCommande(string p_sujet, Commande p_commande)
        {
            if (p_sujet is null)
            {
                throw new ArgumentNullException(nameof(p_sujet));
            }

            if (p_commande is null)
            {
                throw new ArgumentNullException(nameof(p_commande));
            }

            this.Sujet = p_sujet;
            this.Commande = p_commande;
        }

        public static MessageInformationsCommande GenererMessageAleatoire() 
        {
            string[] sujetsPossibles = { "commande.placee.normal", "commande.placee.premium" };
            Random rnd = new Random();

            return new MessageInformationsCommande(sujetsPossibles[rnd.Next(sujetsPossibles.Length)], Commande.GenererCommandeAleatoire());
        }
    }
}
