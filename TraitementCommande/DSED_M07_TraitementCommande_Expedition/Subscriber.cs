using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSED_M07_TraitementCommande_Producteur;

namespace DSED_M07_TraitementCommande_Expedition
{
    internal class Subscriber
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);

        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost" };

        private const string nomEchange = "m07-commandes";

        private const string nomFileMessage = "m07-preparation-expedition";

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
                        Console.WriteLine("***** COMMANDE *****");
                        Console.WriteLine($"Préparer les articles suivants (commande de type {informationsCommande.Sujet.Substring(informationsCommande.Sujet.LastIndexOf('.') + 1)}) : ");
                        foreach (Article article in informationsCommande.Commande.Articles)
                        {
                            Console.WriteLine(article);
                        }
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
    }
}
