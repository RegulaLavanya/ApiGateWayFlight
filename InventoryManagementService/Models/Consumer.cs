using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace InventoryManagementService.Models
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
       // private IServiceProvider _serviceProvider;
       // public AppDbContext _context;

        public Consumer(ILoggerFactory loggerFactory,IServiceProvider serviceProvider)
        {
            this._logger = loggerFactory.CreateLogger<Consumer>();
           // _serviceProvider = serviceProvider;
          //  _context = serviceProvider.CreateScope().ServiceProvider.GetService<AppDbContext>();
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new System.Uri("amqp://guest:guest@localhost:5672")
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("schedule", durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);
           // var consumer = new EventingBasicConsumer(_channel);

           // _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
   

private void HandleMessage(string content)
{
    // we just print this message   
    _logger.LogInformation($"consumer received {content}");
}



public override void Dispose()
{
    _channel.Close();
    _connection.Close();
    base.Dispose();
}

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                var msg = JsonConvert.DeserializeObject<Dictionary<string, int>>(content);
                ConsumerUpdate.UpdateData(Convert.ToInt32(msg["Seats"]), Convert.ToInt32(msg["Id"]));

                // handle the received message  
                HandleMessage(content);
               // _channel.BasicAck(ea.DeliveryTag, false);
            };


            _channel.BasicConsume("schedule", true, consumer);
            return Task.CompletedTask;
        }

        /*public void InventoryConsumer()
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


        }*/
    }
    
}
