using Microsoft.EntityFrameworkCore;
using TelegramRatingBot.Models;

namespace TelegramRatingBot
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=your_server;UserId=your_userId;Password=your_password;database=your_database_name;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
