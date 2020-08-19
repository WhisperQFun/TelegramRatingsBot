using System.Threading.Tasks;
using TelegramRatingBot.Models;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation.Command
{
    public class AddAdmin : IProcessBotCommand
    {
        public string BotCommandName { get { return "AddAdmin"; } }

        public async Task<string> ProcessCommand(BotMessageCommon message)
        {
            using (var _dbContext = new AppDbContext())
            {
                if (message.Admin == null)
                    return "Отказанно";

                var admin = new Admin
                {
                    User = message.ReplyToUser
                };

                _dbContext.Admins.Add(admin);
                await _dbContext.SaveChangesAsync();

                return "Успешно";
            }
        }
    }
}
