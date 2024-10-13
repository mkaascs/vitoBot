using Domain.Abstractions;
using Domain.VitoAPI;

using Application.Configuration;
using Application.DTO;
using Application.Extensions;

namespace Application.Services;

public class MessageBotSendingLogic(BotLogicConfiguration configuration, IMessageApiService messageApiService) {
    private readonly Random _randomizer = new();

    private int UnansweredMessagesQuantity { get; set; } = 0;

    public async Task<IEnumerable<Message>> GetAnswerAsync(MessageDto receivedMessage,
        CancellationToken cancellationToken = default) {
        
        List<Message> answers = [];

        while (_randomizer.WithChance(CalculateFinalChance(answers.Count))) {
            Response<Message> message = await messageApiService.GetRandomMessageAsync
                ((ulong)receivedMessage.Chat.Id, cancellationToken);
            
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

    private double CalculateFinalChance(int answersCount) {
        double finalChance = configuration.DefaultChanceToSendMessage
                             * Math.Log2(UnansweredMessagesQuantity + 2)
                             / (answersCount + 1);

        return finalChance > 1.0
            ? 1.0
            : finalChance;
    }
}