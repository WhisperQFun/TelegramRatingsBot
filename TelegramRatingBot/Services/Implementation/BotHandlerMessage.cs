using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation
{
    public class BotHandlerMessage : IBotHandler, IDisposable
    {
        private ITelegramBotClient _botClient;
        public List<IProcessBotCommand> _botCommands { get; protected set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IPreProcessBotCommand _preProcessBotCommand;

        public BotHandlerMessage(IPreProcessBotCommand preProcessBotCommand, IServiceProvider serviceProvider)
        {
            _preProcessBotCommand = preProcessBotCommand;
            _serviceProvider = serviceProvider;

            var rawCommandsList = _serviceProvider.GetServices(typeof(IProcessBotCommand)).ToList();
            _botCommands = rawCommandsList.ConvertAll(x => (IProcessBotCommand)x);
        }

        public void Dispose() => _botClient.StopReceiving();

        public void StartHandling()
        {
            _botClient = new TelegramBotClient("Paste your token");
            _botClient.OnMessage += OnMessage;
            _botClient.StartReceiving();
        }

        public async void OnMessage(object sender, MessageEventArgs e)
        {
            var botMessage = await _preProcessBotCommand.PreProcessCommand(e.Message);

            if (botMessage != null && botMessage.ReplyToUser != null)
            {
                var currentCommandInstance = _botCommands.FirstOrDefault(o => o.BotCommandName.ToLower() == botMessage.CommandName);

                if (currentCommandInstance != null)
                {
                    var reply = await currentCommandInstance.ProcessCommand(botMessage);

                    if (reply != null)
                        await _botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: reply);
                }
            }
        }
    }
}
