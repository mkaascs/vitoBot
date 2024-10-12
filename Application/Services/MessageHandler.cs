using Application.Abstractions;
using Application.DTO;

namespace Application.Services;

public class MessageHandler : IMessageHandler {
    public void OnGetMessage(MessageDto message)
        => Console.WriteLine($"Chat {message.Chat.Id} - {message.Chat.Title}\n" +
                             $"From {message.From?.Id} - {message.From?.FirstName}\n" +
                             $"{message.Type} - {message.Content}");
}