using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSED_M07_TraitementCommande_Producteur;

namespace DSED_M07_TraitementCommande_Facturation
{
    internal class Subscriber
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);

        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost" };

        private const string nomEchange = "m07-commandes";

        private const string nomFileMessage = "m07-facturation";

        private const string sujetAbonnement = "commande.placee.*";

        public Subscriber()
        {
            ;
        }

        public void TraiterMessages()
        {
            using (IConnection connexion = m_factory.CreateConnection())
            {
                using (IModel channel = connexion.CreateModel())
                {
                    channel.ExchangeDeclare
                        (
                            exchange: nomEchange,
                            type: "topic",
                            durable: true,
                            autoDelete: false
                        );

                    channel.QueueDeclare
                        (
                            queue: nomFileMessage,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                        );

                    channel.QueueBind
                        (
                            queue: nomFileMessage,
                            exchange: nomEchange,
                            routingKey: sujetAbonnement
                        );

                    EventingBasicConsumer consommateur = new EventingBasicConsumer(channel);

                    consommateur.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        string messageRecu = Encoding.UTF8.GetString(body);
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.TypeNameHandling = TypeNameHandling.Auto;
                        MessageInformationsCommande informationsCommande = JsonConvert.DeserializeObject<MessageInformationsCommande>(messageRecu, settings);
                        Facture facture = this.GenererFacture(informationsCommande);
                        this.CreerFichierJSON(informationsCommande.Commande.Reference, JsonConvert.SerializeObject(facture, settings));
                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume
                            (
                                queue: nomFileMessage,
                                autoAck: false,
                                consumer: consommateur
                            );

                    waitHandle.WaitOne();
                }
            }
        }

        private void CreerFichierJSON(Guid p_referenceCommande, string p_contenuJSON)
        {
            string nomFichier = $"{DateTime.Now.ToString("yyyyMMddTHHmmss")}_{p_referenceCommande}_Facture.json";
            string cheminFichier = $"..\\..\\..\\Factures\\{nomFichier}";
            File.WriteAllText(cheminFichier, p_contenuJSON);
        }

        private Facture GenererFacture(MessageInformationsCommande p_informationsCommande)
        {
            decimal totalSansTaxes = 0m;
            decimal totalAvecTaxes = 0m;
            decimal rabaisSiPremium = 0.05m;
            decimal taxesQuebec = 0.15m;

            foreach (Article article in p_informationsCommande.Commande.Articles)
            {
                totalSansTaxes += article.Prix * article.Quantite;
            }

            if (p_informationsCommande.Sujet == "commande.placee.premium") 
            {
                totalSansTaxes -= totalSansTaxes * rabaisSiPremium;
            }

            totalAvecTaxes = totalSansTaxes + (totalSansTaxes * taxesQuebec);

            return new Facture(totalSansTaxes, totalAvecTaxes, p_informationsCommande.Commande.Articles);
        }
    }
}
