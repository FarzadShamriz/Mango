using Microsoft.Azure.Amqp;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text.Json.Serialization;

namespace Mango.Services.OrderAPI.RabbitMQSender
{
    public class RabbitMQOrderMessageSender : IRabbitMQOrderMessageSender
    {

        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQOrderMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }
        public void SendMessage(object message, string exchangeName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(exchange:exchangeName, ExchangeType.Fanout, durable:false);
                var jsonBody = JsonConvert.SerializeObject(message);
                var body = System.Text.Encoding.UTF8.GetBytes(jsonBody);

                channel.BasicPublish(exchange: exchangeName, "", null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }

    }
}
