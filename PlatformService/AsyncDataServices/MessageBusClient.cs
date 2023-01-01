using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;
    public readonly IConnection? _connection;
    private readonly IModel? _channel;

    public MessageBusClient(RabbitMqConfiguration rabbitMqConfiguration)
    {
        _rabbitMqConfiguration = rabbitMqConfiguration ?? throw new ArgumentNullException(nameof(rabbitMqConfiguration));
        var factory = new ConnectionFactory() { HostName = _rabbitMqConfiguration.Host, Port = _rabbitMqConfiguration.Port };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("RabbitMQClient connected to Bus");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in RabbitMQ initializacion {0}", ex.Message);
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("---> rabbitMQ shutdown");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);
        if (_connection is not null && _connection.IsOpen && _channel is not null)
        {
            Console.WriteLine("sending message to RabbitMQ bus");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("connection is closed, not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        if (_channel is not null)
        {

            _channel?.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                 basicProperties: null,
                  body: body);
            Console.WriteLine("Message has been send");
        }
        else
            Console.WriteLine("channel is closed, message has not been send");
    }

    public void Dispose()
    {
        Console.WriteLine("MessageBus Disposed");
        if (_channel is not null && _channel.IsOpen)
        {
            _channel.Close();
            _connection?.Close();
        }
    }
}