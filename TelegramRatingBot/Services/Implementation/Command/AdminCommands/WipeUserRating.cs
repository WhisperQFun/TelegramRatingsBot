using System.Threading.Tasks;
using TelegramRatingBot.Models;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation.Command
{
    public class WipeUserRating : IProcessBotCommand
    {
        public string BotCommandName { get { return "WipeRating"; }}

        public async Task<string> ProcessCommand(BotMessageCommon message)
        {
            using (var _dbContext = new AppDbContext())
            {
                if (message.Admin == null)
                    return "Отказанно";

                message.ReplyToUser.Rating = 100;
                _dbContext.Users.Update(message.ReplyToUser);
                await _dbContext.SaveChangesAsync();
                return "Успешно";
            }
        }
    }
}
