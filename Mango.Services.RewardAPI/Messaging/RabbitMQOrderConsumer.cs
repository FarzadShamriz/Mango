using Mango.Services.RewardAPI.Messages;
using Mango.Services.RewardAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Mango.Services.RewardAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {

        private readonly RewardService _rewardsService;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _exchangeName;
        private readonly string _queueName;

        public RabbitMQOrderConsumer(IConfiguration configuration, RewardService rewardsService)
        {
            
            _configuration = configuration;
            _rewardsService = rewardsService;
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
            _rewardsService.UpdateRewards(reward).GetAwaiter().GetResult();
        }

    }
}
