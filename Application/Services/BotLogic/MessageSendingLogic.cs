using Domain.Abstractions;
using Domain.VitoAPI;

using Application.DTO;
using Application.Extensions;
using Domain.Entities;

namespace Application.Services.BotLogic;

public class MessageSendingLogic(IMessageApiService messageApiService) {
    private readonly Random _randomizer = new();

    private int UnansweredMessagesQuantity { get; set; } = 0;

    public async Task<IEnumerable<Message>> GetAnswerAsync(MessageDto receivedMessage, UserSettings userSettings,
        CancellationToken cancellationToken = default) {
        
        List<Message> answers = [];

        while (_randomizer.WithChance(CalculateFinalChance(userSettings.DefaultChanceToSendMessage, answers.Count))) {
            Response<Message> message = await messageApiService.GetRandomMessageAsync
                (receivedMessage.Chat.Id, cancellationToken);
            
            if (string.IsNullOrWhiteSpace(message.Content?.Content))
                break;

            Message? alreadyExistingAnswer = answers.Find(answer
                => answer.Content == message.Content.Content && answer.Type == message.Content.Type);

            if (alreadyExistingAnswer is not null) 
                continue;
            
            answers.Add(message.Content);
            UnansweredMessagesQuantity = 0;
        }

        if (answers.Count == 0)
            UnansweredMessagesQuantity++;
        
        return answers;
    }

    private decimal CalculateFinalChance(decimal defaultChanceToSendMessage, int answersCount) {
        decimal finalChance = defaultChanceToSendMessage
                              * (decimal)Math.Log2(UnansweredMessagesQuantity + 2)
                              / (answersCount + 1);

        return finalChance > 1m
            ? 1m
            : finalChance;
    }
}