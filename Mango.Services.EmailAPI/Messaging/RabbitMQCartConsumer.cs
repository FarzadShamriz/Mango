using Mango.Services.EmailAPI.Models.DTOs;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQCartConsumer : BackgroundService
    {

        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _queueName;

        public RabbitMQCartConsumer(IConfiguration configuration, EmailService emailService)
        {
            
            _configuration = configuration;
            _emailService = emailService;
            _queueName = configuration.GetValue<string>("TopicAndQueueNames:emailshoppingcarttopic");
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, false, false, false, null);

        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(content);
                HandleMessage(cartDto).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CartDto cartDto)
        {
            _emailService.EmailCartAndLog(cartDto).GetAwaiter().GetResult();
        }

    }
}
