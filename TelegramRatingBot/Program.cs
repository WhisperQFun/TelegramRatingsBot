using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TelegramRatingBot.Services.Implementation;
using TelegramRatingBot.Services.Interfaces;

namespace TelegramRatingBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = ConfigureServices();

            var botHandler = service.GetService<IBotHandler>();
            botHandler.StartHandling();

            Thread.Sleep(int.MaxValue);
        }

        private static IServiceProvider ConfigureServices()
        {
            var types = from t in Assembly.GetAssembly(typeof(IProcessBotCommand)).GetTypes().Where(t => t.GetInterfaces()
                       .Contains(typeof(IProcessBotCommand)))
                        select Activator.CreateInstance(t) as IProcessBotCommand;

            var services = new ServiceCollection();
            var config = GetConfiguration();

            foreach (var type in types) 
            {
                services.AddSingleton(typeof(IProcessBotCommand), type);
            }

            services.AddLogging();

            services.AddSingleton<IPreProcessBotCommand, PreProcessBotCommand>();
            services.AddSingleton<IBotHandler, BotHandlerMessage>();
            
            return services.BuildServiceProvider(true);
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}
