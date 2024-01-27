
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using service2.Models;

namespace service2.Services
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, EventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {
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
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName,
                    exchange: "trigger",
                    routingKey: "");
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                System.Console.WriteLine("Init RMQ COMPLETE!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Init Failed with {ex.Message}!");
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                System.Console.WriteLine("Event received ----> ");
                var body = ea.Body;
                try
                {
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                    var eventData = JsonSerializer.Deserialize<Post>(notificationMessage);
                    System.Console.WriteLine(eventData);
                    _eventProcessor.HandleEvent(eventData);
                }catch(Exception ex){
                    System.Console.WriteLine(ex.Message);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Showdown");
        }
        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}