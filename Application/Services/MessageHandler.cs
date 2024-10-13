using Application.Abstractions;

using Application.DTO;
using Application.DTO.Commands;

namespace Application.Services;

public class MessageHandler(IMessageSender messageSender) : IMessageHandler {
    public async Task OnGetMessageAsync(MessageDto message) {
        if (string.IsNullOrWhiteSpace(message.Content))
            return;
        
        Console.WriteLine($"{message.Date:T}\n" +
                          $"From {message.From?.Id} - {message.From?.FirstName}\n" +
                          $"{message.Type} - {message.Content}");
        
        await messageSender.SendMessageAsync(new SendMessageCommand(
            message.Chat.Id,
            message.Content,
            message.Type));
    }
}