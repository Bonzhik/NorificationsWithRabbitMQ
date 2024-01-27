using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using service1.Dtos;
using service1.Models;

namespace service1.Services.RabbitMQService
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            System.Console.WriteLine($"ПОДКЛЮЧЕНИЕ ПО {factory.HostName} , {factory.Port}");
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                System.Console.WriteLine("Connect to RabbitMQ succces");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"While starting RabbitMQ ---> {ex.Message}");
            }
        }
        public void PublishMessage(PostToMessage post)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var message = JsonSerializer.Serialize(post, options);

            if (_connection.IsOpen)
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(
                    exchange: "trigger",
                    routingKey: "",
                    basicProperties: null,
                    body: body
                );

            }
            else { }
        }
        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Showdown");
        }
    }
}