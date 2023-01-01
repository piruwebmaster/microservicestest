namespace CommandsService.Models;
public class Command
{
    public string HowTo = string.Empty;
    public string CommandLine = string.Empty;
    public int PlatformId;
    public Platform Platform = default!;
    public int Id = default;

}