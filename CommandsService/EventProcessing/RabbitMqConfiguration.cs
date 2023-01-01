namespace CommandsService.EventProcessing;

public class RabbitMQConfiguration
{
    public static string NAME { get; set; } = "Rabbit";
    public string Host { get; init; } = String.Empty;
    public int Port { get; init; }
}