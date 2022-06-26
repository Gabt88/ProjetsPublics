using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSED_M07_TraitementCommande_Producteur
{
    internal class Publisher
    {
        private ConnectionFactory m_factory = new ConnectionFactory() { HostName = "localhost" };

        private const string nomEchange = "m07-commandes";
        public Publisher()
        {
            ;
        }

        public void EnvoyerMessagesAleatoires(int p_nombreMessages)
        {
            if (p_nombreMessages <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(p_nombreMessages));
            }

            using (IConnection connection = this.m_factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                            exchange: nomEchange,
                            type: "topic",
                            durable: true,
                            autoDelete: false
                        );

                    for (int i = 0; i < p_nombreMessages; i++)
                    {
                        MessageInformationsCommande nouveauMessage = MessageInformationsCommande.GenererMessageAleatoire();
                        byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(nouveauMessage));

                        channel.BasicPublish(
                                exchange: nomEchange,
                                routingKey: nouveauMessage.Sujet,
                                basicProperties: null,
                                body: body
                            );
                    }
                }
            }
        }
    }
}
