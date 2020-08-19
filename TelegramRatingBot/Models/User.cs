namespace TelegramRatingBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public bool IsProtected { get; set; }
    }
}
