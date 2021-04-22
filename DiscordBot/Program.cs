using Discord;
using Discord.Commands;
using Discord.WebSocket;
using AvatarBot.Bot.Tools;
using AvatarBot.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AvatarBot.Bot
{
    class Program
    {

        CommandHandler _handler;
            
        static void Main(string[] args)
        => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {

            /** Load the localization strings */
            Localization.LoadData();

            /** Set the DB Context connection string **/
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
            AvatarBotContext.ConnectionString = config.GetConnectionString("local");

            var token = config.GetValue<string>("bot_token");
            if (token == null || token == "")
            {
                Console.WriteLine($"Token is absent or invalid. Please check configuration.");
                Console.ReadLine();
                return;
            }

            /* Discord */
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            client.Log += Log;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            var commands = new CommandService();
            var services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            _handler = new CommandHandler(commands, client, services);
            await _handler.SetupAsync(client);

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            var c = Console.ForegroundColor;

            switch (msg.Severity)
            {
                case LogSeverity.Critical: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogSeverity.Info: Console.ForegroundColor = ConsoleColor.White; break;
                case LogSeverity.Verbose: Console.ForegroundColor = c; break;
                case LogSeverity.Debug: Console.ForegroundColor = c; break;
            }
            Console.WriteLine(String.Format("{0:yy-MM-dd HH:mm:ss} [{1,8}] {2,10}: {3}", DateTime.Now, msg.Severity, msg.Source, msg.Message));

            Console.ForegroundColor = c;
            return Task.CompletedTask;
        }
    }
}
