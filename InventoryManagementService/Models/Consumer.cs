using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementService.Models
{
    public class Consumer
    {
        public void InventoryConsumer()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new System.Uri("amqp://guest:guest@localhost:5672")
            };


            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            // QueueConsumer.Consume(channel);

            channel.QueueDeclare("queue1", durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (Sender, e) =>
                     {
                         var body = e.Body.ToArray();
                         var message = Encoding.UTF8.GetString(body);
                         Console.WriteLine(message);
                     };

            channel.BasicConsume("queue1", true, consumer);
            Console.WriteLine("consumer started");
            Console.ReadLine();


        }
    }
    
}
