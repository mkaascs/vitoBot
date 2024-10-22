using Microsoft.Extensions.Logging;

using Application.Abstractions;
using Application.Abstractions.BotCommands;
using Application.DTO.Commands;
using Application.Services.BotLogic;

namespace Application.Services;

<<<<<<< HEAD
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
=======
public class MessageHandler(MessageBotLogic botLogic, ILogger<MessageHandler> logger) : IMessageHandler {
    public async Task OnGetMessageAsync(IMessageHandlingContext context, CancellationToken cancellationToken = default) {
        if (context is IBotCommandHandlingContext)
            return;
        
        IList<SendMessageCommand> answers = (await botLogic.GetAnswerAsync(context.Message, cancellationToken)).ToList();

        IList<Task> replyingTasks = new List<Task>(answers.Count);
        foreach (SendMessageCommand answer in answers)
            replyingTasks.Add(context.AnswerAsync(answer, cancellationToken));

        await Task.WhenAll(replyingTasks);
        
        logger.LogInformation("{count} messages were sent to {chat}", answers.Count, context.Message.From?.FirstName);
>>>>>>> development
    }
}