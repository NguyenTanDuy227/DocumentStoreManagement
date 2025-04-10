using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQOrder.ConsoleApp
{
    public class OrderConsumer(IRepository<Order> mongoOrderRepository) : IHostedService
    {
        private readonly IRepository<Order> _mongoOrderRepository = mongoOrderRepository;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Here we specify the Rabbit MQ Server. we use rabbitMQ docker image and use it
            ConnectionFactory factory = new()
            {
                HostName = "localhost"
            };

            // Create the RabbitMQ connection using connection factory details as i mentioned above
            IConnection connection = await factory.CreateConnectionAsync(cancellationToken);

            // Here we create channel with session and model
            using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            // Declare the queue after mentioning name and a few property related to that
            await channel.QueueDeclareAsync("order", exclusive: false, cancellationToken: cancellationToken);

            // Set Event object which listen message from channel which is sent by producer
            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                byte[] body = eventArgs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Order message received: {message}");

                // Insert new order
                await InsertOrderAsync(message);
            };

            // Read the message
            await channel.BasicConsumeAsync(queue: "order", autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
            Console.ReadKey();
        }

        private async Task InsertOrderAsync(string message)
        {
            // Insert order into MongoDB
            Order order = JsonConvert.DeserializeObject<Order>(message) ?? throw new Exception("Deserialize order failed!");
            await _mongoOrderRepository.AddAsync(order);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Implement any cleanup logic here
            // ...

            return Task.CompletedTask;
        }
    }
}