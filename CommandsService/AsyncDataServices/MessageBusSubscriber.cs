using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataService;

public class MessageBusSubscriber : BackgroundService
{
    private readonly RabbitMQConfiguration _rabbitMQConfiguration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection? _connection;
    private IModel? _channel;
    private string? _queueName;

    public MessageBusSubscriber(RabbitMQConfiguration rabbitMQConfiguration, IEventProcessor eventProcessor)
    {
        _rabbitMQConfiguration = rabbitMQConfiguration;
        _eventProcessor = eventProcessor;

        InitlizeRabbitMQ();
    }

    private void InitlizeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQConfiguration.Host,
            Port = _rabbitMQConfiguration.Port
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
        Console.WriteLine("--> Listening on the Message Bus...");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection Shutdown");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ModuleHandle, ea) =>
        {
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            _eventProcessor.ProccesEvent(notificationMessage);
        };
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;

    }

    public override void Dispose()
    {
        if (_channel is not null && _channel.IsOpen)
        {
            _channel.Close();
        }
        base.Dispose();
    }
}