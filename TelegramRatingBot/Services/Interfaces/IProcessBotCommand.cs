using System.Threading.Tasks;
using TelegramRatingBot.Models;

namespace TelegramRatingBot.Services.Interfaces
{
    public interface IProcessBotCommand
    {
        string BotCommandName { get; }
        Task<string> ProcessCommand(BotMessageCommon message);
    }
}
