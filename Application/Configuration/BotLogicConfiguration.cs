using System.ComponentModel.DataAnnotations;

namespace Application.Configuration;

public class BotLogicConfiguration {
    /// <summary>
    /// A value between 0 and 1.0 defining default chance to send message
    /// </summary>
    [Range(0.0, 1.0)]
    public double DefaultChanceToSendMessage { get; init; }
    
    /// <summary>
    /// A value between 0 and 1.0 defining chance to save text message
    /// </summary>
    [Range(0.0, 1.0)]
    public double ChanceToSaveTextMessage { get; init; }
    
    /// <summary>
    /// A value between 0 and 1.0 defining chance to save non-text message
    /// </summary>
    [Range(0.0, 1.0)]
    public double ChanceToSaveMessage { get; init; }
}