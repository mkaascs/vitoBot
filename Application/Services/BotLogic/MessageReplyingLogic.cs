using Domain.Abstractions;
using Domain.Entities;
using Domain.VitoAPI;

using Application.DTO;
using Application.DTO.Commands;
using Application.Extensions;

namespace Application.Services.BotLogic;

public class MessageReplyingLogic(IMessageApiService messageApiService) {
    private readonly Random _randomizer = new();
    
    public async Task<IEnumerable<SendMessageCommand>> GetAnswerAsync(MessageDto receivedMessage, UserSettings userSettings,
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
        }
        
        return answers.Select(answer => new SendMessageCommand(answer.Content, answer.Type));
    }

    private decimal CalculateFinalChance(decimal defaultChanceToSendMessage, int answersCount) {
        decimal finalChance = defaultChanceToSendMessage
                              / (answersCount + 1);

        return finalChance > 1m
            ? 1m
            : finalChance;
    }
}