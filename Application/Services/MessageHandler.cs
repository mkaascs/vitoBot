using Microsoft.Extensions.Logging;

using Application.Abstractions;
using Application.DTO;
using Application.DTO.Commands;
using Application.Services.BotCommands;
using Application.Services.BotLogic;

namespace Application.Services;

public class MessageHandler(BotCommandHandler commandHandler, MessageBotLogic botLogic, IMessageSender messageSender, ILogger<MessageHandler> logger) : IMessageHandler {
    public async Task OnGetMessageAsync(MessageDto receivedMessage, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(receivedMessage.Content))
            return;

        if (await commandHandler.TryExecuteCommand(receivedMessage, cancellationToken)) {
            logger.LogInformation("The command {name} was executed", receivedMessage.Content);
            return;
        }

        SendMessageCommand[] answers = (await botLogic.OnGetMessageAsync(receivedMessage, cancellationToken)).ToArray();

        List<Task> sendingAnswerTasks = new(answers.Length);
        sendingAnswerTasks.AddRange(
            answers.Select(answer => messageSender.SendMessageAsync(answer, cancellationToken)));

        await Task.WhenAll(sendingAnswerTasks);
        
        logger.LogInformation("{count} messages were sent to {chat}", answers.Length, receivedMessage.From?.FirstName);
    }
}