namespace CommandsService.Models;
public class Platform
{
    public string Name = String.Empty;
    public int ExternalId;
    public ICollection<Command> Commands = default!;
    public int Id = default;
}
