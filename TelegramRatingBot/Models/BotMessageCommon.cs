using System.Collections.Generic;

namespace TelegramRatingBot.Models
{
    public class BotMessageCommon
    {
        public string CommandName { get; set; }

        public List<string> CleanMessage { get; set; }

        public User User { get; set; }

        public string UserNickname { get; set; }

        public User ReplyToUser { get; set; }

        public Admin Admin { get; set; }
    }
}
