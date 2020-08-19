using System.Threading.Tasks;
using TelegramRatingBot.Models;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation.Command
{
    public class ChangeNickname : IProcessBotCommand
    {
        public string BotCommandName { get { return "AddAdmin"; } }

        public async Task<string> ProcessCommand(BotMessageCommon message)
        {
            using (var _dbContext = new AppDbContext())
            {
                if (message.UserNickname == null)
                    return "Отказанно";

                message.User.Name = message.UserNickname;
                _dbContext.Users.Update(message.User);
                await _dbContext.SaveChangesAsync();
                return "Успешно";
            }
        }
    }
}
