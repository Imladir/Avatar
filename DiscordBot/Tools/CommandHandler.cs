using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using AvatarBot.Model;
using System.Linq;
using AvatarBot.Bot.Tools.ParamReaders;
using AvatarBot.Model.Servers;
using AvatarBot.Model.Characters;
using AvatarBot.Model.Identity;

namespace AvatarBot.Bot.Tools
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandler(CommandService commands, DiscordSocketClient client, IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
        }

        public async Task SetupAsync(DiscordSocketClient _)
        {
            _client.MessageReceived += CommandHandleAsync;

            // Adding custom type reader
            _commands.AddTypeReader(typeof(CommandOptions<Model.Characters.Character>), new CommandOptionsReader<Model.Characters.Character>());
            _commands.AddTypeReader(typeof(CommandOptions<PC>), new CommandOptionsReader<PC>());
            _commands.AddTypeReader(typeof(CommandOptions<NPC>), new CommandOptionsReader<NPC>());
            _commands.AddTypeReader(typeof(List<int>), new IntListReader());
            _commands.AddTypeReader(typeof(PrivateCharacter), new PrivateCharacterReader());
            _commands.AddTypeReader(typeof(GeneralCharacter), new CharacterReader());

            // Registering modules
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task CommandHandleAsync(SocketMessage messageParam)
        {
            try {
                // Don't process the command if it was a system message
                if (!(messageParam is SocketUserMessage message)) return;

                // Ignore the command if it's not on a Server
                if (!(message.Channel is SocketGuildChannel)) return;

                var guild = ((SocketGuildChannel)message.Channel).Guild;
                ulong serverID = guild.Id;

                // Create a number to track where the prefix ends and the command begins
                int argPos = 0;

                char prefix = '!';
                using (var ctx = new AvatarBotContext())
                {
                    Server server;
                    if (!ctx.Servers.Any(x => x.DiscordID == (long)serverID))
                    {
                        server = new Server() { DiscordID = (long)serverID, Name = guild.Name };
                        ctx.Servers.Add(server);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        server = ctx.Servers.Single(x => x.DiscordID == (long)serverID);
                        if (server.Name == null || server.Name != guild.Name)
                        {
                            server.Name = guild.Name;
                            ctx.SaveChanges();
                        }
                    }
                    prefix = server.Prefix[0];
                }

                // If the user just says 'prefix' I'll assume he wants to know the prefix used by the bot.
                if (messageParam.Content == "prefix")
                    await messageParam.Channel.SendMessageAsync($"I assume you're asking for my prefix? It's '{prefix}'.");

                // Determine if the message is a command based on the prefix and make sure no bots trigger commands
                if (!(message.HasCharPrefix(prefix, ref argPos) ||
                    message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                    message.Author.IsBot)
                    return;

                if (!message.Content.StartsWith($"{prefix}say"))
                    Console.WriteLine($"Received command from {message.Author.Username}: {message}");

                // Check if the user is a registered player, create him if he isn't

                // Create a WebSocket-based command context based on the message
                var context = new SocketCommandContext(_client, message);

                // Make sure the user exists / is up to date
                using (var ctx = new AvatarBotContext())
                {
                    var player = ctx.Users.SingleOrDefault(x => x.DiscordID == (long)message.Author.Id);
                    if (player == null)
                    {
                        player = new User() { DiscordID = (long)message.Author.Id, UserName = message.Author.Username };
                        ctx.Users.Add(player);
                        ctx.SaveChanges();
                    }
                    if (message.Author.Username != player.UserName)
                    {
                        player.UserName = message.Author.Username;
                        ctx.SaveChanges();
                    }
                    if (player.Verbose) await context.User.SendMessageAsync($"Received command:\n{message.Content}");
                    ctx.Entry(player).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }

                // Execute the command with the command context we just
                // created, along with the service provider for precondition checks.

                // Keep in mind that result does not indicate a return value
                // rather an object stating if the command executed successfully.
                var result = await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null);

                // Optionally, we may inform the user if the command fails
                // to be executed; however, this may not always be desired,
                // as it may clog up the request queue should a user spam a
                // command.
                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ErrorReason);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in Handler: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
