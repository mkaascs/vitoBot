using Domain.Abstractions;
using Domain.VitoAPI;

using Application.Configuration;
using Application.DTO;
using Application.Extensions;

namespace Application.Services.BotLogic;

public class MessageSavingLogic(BotLogicConfiguration configuration, IChatApiService chatApiService, IMessageApiService messageApiService) {
    private readonly Random _randomizer = new();
    
    public async Task<bool> TryRememberMessageAsync(MessageDto receivedMessage,
        CancellationToken cancellationToken = default) {

        if (receivedMessage.Type == ContentType.Text &&
            _randomizer.WithChance(configuration.ChanceToSaveTextMessage)) {
            
            return await RememberMessageAsync(receivedMessage, cancellationToken);
        }

        if (receivedMessage.Type != ContentType.Text &&
            _randomizer.WithChance(configuration.ChanceToSaveMessage)) {
            
            return await RememberMessageAsync(receivedMessage, cancellationToken);
        }

        return false;
    }

    private async Task<bool> RememberMessageAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(receivedMessage.Content))
            return false;

        await RegisterNewChatIfRequiredAsync(receivedMessage, cancellationToken);
        Response<bool> response = await messageApiService.AddNewMessageAsync(
            receivedMessage.Chat.Id,
            new Message(receivedMessage.Content, receivedMessage.Type),
            cancellationToken);

        return response.Content;
    }

    private async Task RegisterNewChatIfRequiredAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        Response<IEnumerable<Chat>> registeredChats = 
            await chatApiService.GetChatsAsync(cancellationToken);
        
        Chat? chatFromReceivedMessage = registeredChats.Content?
            .FirstOrDefault(chat => chat.Id.Equals(receivedMessage.Chat.Id));
        
        if (chatFromReceivedMessage != null)
            return;

        string? newChatName = string.IsNullOrEmpty(receivedMessage.Chat.Title)
            ? receivedMessage.From?.FirstName
            : receivedMessage.Chat.Title;

        await chatApiService.RegisterNewChatAsync(
            new Chat(receivedMessage.Chat.Id, newChatName),
            cancellationToken);
    }
}