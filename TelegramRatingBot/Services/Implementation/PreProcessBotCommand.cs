using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramRatingBot.Models;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot.Services.Implementation
{
    public class PreProcessBotCommand : IPreProcessBotCommand
    {
        public async Task<BotMessageCommon> PreProcessCommand(Message message)
        {
            BotMessageCommon result = new BotMessageCommon();

            using (var _dbContext = new AppDbContext())
            {
                if (message.ReplyToMessage != null && message.Text != null && message.Text.Length < 1000 && message.ReplyToMessage.From.Id != message.From.Id && message.ReplyToMessage.From.IsBot == false)
                {
                    var messageText = message.Text.Split(' ');

                    if (messageText[0].StartsWith('/') == true && message.Text.Length < 70) 
                    {
                        result.CommandName = messageText[0].Replace("/", "").ToLower();

                        var cleanMessageText = messageText.Where(o => !o.Contains('/') && !o.Contains('@')).Take(2).ToList();

                        var nickname = "";

                        if (cleanMessageText.Count == 1) 
                        {
                            nickname = cleanMessageText[0];
                        }

                        foreach (var messageItem in cleanMessageText) 
                        {
                            nickname = nickname + " " + messageItem;
                        }

                        result.UserNickname = nickname;

                        result.User = await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == message.From.Id);

                        result.ReplyToUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == nickname.Trim());

                        result.Admin = await _dbContext.Admins.FirstOrDefaultAsync(u => u.User == result.User);

                        result.CleanMessage = cleanMessageText;

                        return result;
                    }

                    if (messageText[0].StartsWith('+') == true || message.Text.StartsWith("-") == true) 
                    {
                        var cleanMessage = message.Text.Split(' ').ToList();

                        result.CommandName = "ChangeRating".ToLower();

                        result.ReplyToUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == message.ReplyToMessage.From.Id);

                        var username = "";

                        if (result.ReplyToUser == null) 
                        {
                            try
                            {
                                if (message.ReplyToMessage.From.FirstName == "" && message.ReplyToMessage.From.LastName == "")
                                {
                                    username = message.ReplyToMessage.From.Username;
                                }
                                else
                                {
                                    username = message.ReplyToMessage.From.FirstName + " " + message.ReplyToMessage.From.LastName;
                                }
                            }
                            catch
                            {
                                username = "Одаренный";
                            }
                        }

                        var userRanked = new Models.User
                        {
                            TelegramId = message.ReplyToMessage.From.Id,
                            Name = username,
                            Rating = 100,
                            IsProtected = false
                        };

                        _dbContext.Users.Add(userRanked);
                        await _dbContext.SaveChangesAsync();

                        result.CleanMessage = cleanMessage;

                        return result;
                    }
                }
            }
            return result;
        }
    }
}
