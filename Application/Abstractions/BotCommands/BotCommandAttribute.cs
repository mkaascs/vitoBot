namespace Application.Abstractions.BotCommands;

public class BotCommandAttribute : Attribute {
    private string _botCommandName = "command";
    private string _botCommandDescription = "some command";
    
    public BotCommandAttribute() { }

    public BotCommandAttribute(string commandName) {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandName);
        _botCommandName = commandName;
    }

    public BotCommandAttribute(string commandName, string description) : this(commandName) {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        _botCommandDescription = description;
    }

    public string CommandName {
        get => _botCommandName;
        set {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _botCommandName = value;
        }
    }

    public string Description {
        get => _botCommandDescription;
        set {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _botCommandDescription = value;
        }
    }
}