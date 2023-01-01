namespace CommandsService.EventProcessing;

public interface IEventProcessor
{
    Task ProccesEvent(string message);
}