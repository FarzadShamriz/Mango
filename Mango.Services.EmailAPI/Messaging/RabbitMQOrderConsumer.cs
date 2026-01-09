using Mango.Services.EmailAPI.Messages;
using Mango.Services.EmailAPI.Models.DTOs;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Mango.Services.EmailAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {

        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public RabbitMQOrderConsumer(IConfiguration configuration, EmailService emailService)
        {
            
            _configuration = configuration;
            _emailService = emailService;
            _exchangeName = configuration.GetValue<string>("TopicAndQueueNames:OrderCreateTopic");
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchangeName, ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, _exchangeName, "");
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardMessage reward = JsonConvert.DeserializeObject<RewardMessage>(content);
                HandleMessage(reward).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardMessage reward)
        {
            _emailService.LogOrderPlaced(reward).GetAwaiter().GetResult();
        }

    }
}
