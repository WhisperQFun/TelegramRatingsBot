using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramRatingBot.Models;

namespace TelegramRatingBot.Services.Interfaces
{
    public interface IPreProcessBotCommand
    {
        Task<BotMessageCommon> PreProcessCommand(Message message);
    }
}
