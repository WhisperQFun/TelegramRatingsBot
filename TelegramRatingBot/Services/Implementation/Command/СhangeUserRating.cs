using System.Threading.Tasks;
using TelegramRatingBot.Models;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation.Command
{
    public class СhangeUserRating : IProcessBotCommand
    {
        public string BotCommandName { get { return "ChangeRating"; } }

        public async Task<string> ProcessCommand(BotMessageCommon message)
        {
            using (var _dbContext = new AppDbContext())
            {
                int plusCount = 0;
                foreach (char c in message.CleanMessage[0])
                    if (c == '+') plusCount++;

                int minusCount = 0;
                foreach (char c in message.CleanMessage[0])
                    if (c == '-') minusCount++;

                if (message.ReplyToUser != null)
                {
                    if (plusCount > minusCount)
                    {
                        if (plusCount > 5) plusCount = 5;

                        message.ReplyToUser.Rating += plusCount;
                    }
                    if (plusCount < minusCount)
                    {
                        if (minusCount > 3) minusCount = 3;

                        message.ReplyToUser.Rating -= minusCount;
                    };

                    _dbContext.Users.Update(message.ReplyToUser);
                    await _dbContext.SaveChangesAsync();

                    return "Rating " + message.ReplyToUser.Name + ":\n" + message.ReplyToUser.Rating.ToString();
                }
                return "";
            }
        }
    }
}
