using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.DTO;
using Application.DTO.Commands;
using Application.Extensions;

namespace Application.Services.BotLogic;

public class MessageReplyingLogic(IMessageApiService messageApiService) {
    private readonly Random _randomizer = new();
    
    private Dictionary<ulong, int> ChatsWaitingOnResponse { get; set; } = new();
    
    public async Task<IEnumerable<SendMessageCommand>> GetAnswerAsync(MessageDto receivedMessage, UserSettings userSettings,
        CancellationToken cancellationToken = default) {
        
        List<Message> answers = [];
        
        while (_randomizer.WithChance(CalculateFinalChance(receivedMessage.Chat.Id, userSettings.DefaultChanceToSendMessage, answers.Count))) {
            Response<Message> message = await messageApiService.GetRandomMessageAsync
                (receivedMessage.Chat.Id, cancellationToken);
            
            if (string.IsNullOrWhiteSpace(message.Content?.Content))
                break;

            Message? alreadyExistingAnswer = answers.Find(answer
                => answer.Content == message.Content.Content && answer.Type == message.Content.Type);

            if (alreadyExistingAnswer is not null) 
                continue;
            
            answers.Add(message.Content);
            if (ChatsWaitingOnResponse.ContainsKey(receivedMessage.Chat.Id))
                ChatsWaitingOnResponse[receivedMessage.Chat.Id] = 0;
        }
        
        if (answers.Count > 0)
            return answers.Select(answer => new SendMessageCommand(answer.Content, answer.Type));

        if (!ChatsWaitingOnResponse.TryAdd(receivedMessage.Chat.Id, 1))
            ChatsWaitingOnResponse[receivedMessage.Chat.Id]++;
        
        return [];
    }

    private decimal CalculateFinalChance(ulong chatId, decimal defaultChanceToSendMessage, int answersCount) {
        ChatsWaitingOnResponse.TryGetValue(chatId, out int nonAnsweredMessages);
        
        decimal finalChance = defaultChanceToSendMessage
                              * (decimal)Math.Log2(nonAnsweredMessages + 2)
                              / (answersCount + 1);
        
        return finalChance > 1m
            ? 1m
            : finalChance;
    }
}