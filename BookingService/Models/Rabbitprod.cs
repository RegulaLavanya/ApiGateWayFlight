using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Models
{
    public class Rabbitprod
    {
        public static void SendMessage<T>(T Message)
        {
            var factory = new ConnectionFactory
            {
                Uri = new System.Uri("amqp://guest:guest@localhost:5672")
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //   QueueProducer.Publish(channel);

            channel.QueueDeclare("schedule", durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);
            var count = 10;
            var message = new { Name = "Producer", Message = $"ScheduleId:{ Message }" };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", "schedule", null, body);
        }
     
         
    }
}
