using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_Journal
{
    internal class Subscriber
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);

        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost" };

        private const string nomEchange = "m07-commandes";

        private const string nomFileMessage = "m07-journal";

        private const string sujetAbonnement = "#";

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
                        string message = Encoding.UTF8.GetString(body);
                        this.CreerFichierJSON(message);
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
        private void CreerFichierJSON(string p_contenuJSON)
        {
            string nomFichier = $"{DateTime.Now.ToString("yyyyMMddTHHmmss")}_Nouveau_{Guid.NewGuid()}.json";
            string cheminFichier = $"..\\..\\..\\Journal\\{nomFichier}";
            File.WriteAllText(cheminFichier, p_contenuJSON);
        }
    }
}
