using Microsoft.Azure.Amqp;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text.Json.Serialization;

namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
    {

        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQAuthMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }
        public void SendMessage(object message, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, null);
            var jsonBody = JsonConvert.SerializeObject(message);
            var body = System.Text.Encoding.UTF8.GetBytes(jsonBody);

            channel.BasicPublish(exchange: "", routingKey: queueName, null, body: body);

        }
    }
}
