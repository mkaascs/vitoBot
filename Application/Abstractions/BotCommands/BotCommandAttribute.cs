namespace Application.Abstractions.BotCommands;

/// <summary>
/// Required attribute of the bot command which represents general information about the command
/// </summary>
public class BotCommandAttribute : Attribute
{
    public BotCommandAttribute() { }

    public BotCommandAttribute(string commandName, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        CommandName = commandName;
        Description = description;
    }

    public string CommandName { get; set; } = "command";

    public string Description { get; set; } = "some command";
}