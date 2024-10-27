using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.DTO;
using Application.DTO.Commands;
using Application.Extensions;

namespace Application.Services.BotLogic;

public class MessageReplyingLogic(IMessageApiService messageApiService)
{
    private readonly Random _randomizer = new();
    
    private Dictionary<ulong, int> ChatsWaitingOnResponse { get; } = new();
    
    public async Task<IEnumerable<SendMessageCommand>> GetAnswerAsync(
        MessageDto receivedMessage,
        UserSettings userSettings,
        CancellationToken cancellationToken = default)
    {
        List<Message> answers = [];
        ChatsWaitingOnResponse.TryGetValue(receivedMessage.Chat.Id, out int unansweredMessageCount);
        
        while (_randomizer.WithChance(CalculateFinalChance(userSettings.DefaultChanceToSendMessage, unansweredMessageCount, answers.Count)))
        {
            Message? randomMessage = await messageApiService
                .GetRandomMessageAsync(receivedMessage.Chat.Id, cancellationToken);
            
            if (string.IsNullOrWhiteSpace(randomMessage?.Content))
                break;

            bool messageAlreadyExist = answers.Any(answer
                => answer.Content == randomMessage.Content && answer.Type == randomMessage.Type);

            if (messageAlreadyExist) 
                continue;
            
            answers.Add(randomMessage);
            ChatsWaitingOnResponse.Remove(receivedMessage.Chat.Id);
        }
        
        if (answers.Count > 0)
            return answers.Select(answer => new SendMessageCommand(answer.Content, answer.Type));

        ChatsWaitingOnResponse.TryAdd(receivedMessage.Chat.Id, 0);
        ChatsWaitingOnResponse[receivedMessage.Chat.Id]++;
        return [];
    }

    private decimal CalculateFinalChance(decimal defaultChanceToSendMessage, int unansweredMessageCount, int answersCount)
    {
        decimal finalChance = defaultChanceToSendMessage
                              * (decimal)Math.Log2(unansweredMessageCount + 2)
                              / (answersCount + 1);
        
        return finalChance > 1m
            ? 1m
            : finalChance;
    }
}
