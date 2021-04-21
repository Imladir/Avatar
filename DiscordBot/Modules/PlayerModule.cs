using Discord;
using Discord.Commands;
using Discord.WebSocket;
using AvatarBot.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using AvatarBot.Bot.Tools;

namespace AvatarBot.Bot.Modules
{
    [Group("player")]
    public class Players : ModuleBase<SocketCommandContext>
    {
        [Command("defaultcharacter")]
        [Alias("default")]
        [Summary("Sets the user's default character.")]
        public async Task SetDefaultCharacter([Remainder]
                                              [Summary("\nAlias of the user to become the new default character.\n" +
                                                       "It must be an existing character that the player owns.")]
                                              string nameOrAlias)
        {
            using var ctx = new AvatarBotContext();
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                //player.LoadDefaultCharacters(ctx);
                var character = ctx.GetPlayerCharacter(Context.Guild.Id, Context.User.Id, nameOrAlias);
                if (character == null)
                {
                    throw new Exception(Localization.Format(server.Localization, "character_not_found", nameOrAlias));
                }
                var defChar = player.DefaultCharacters.SingleOrDefault(x => x.Server.DiscordID == (long)Context.Guild.Id);
                if (defChar == null)
                {
                    defChar = new Model.Servers.DefaultCharacter()
                    {
                        Player = player,
                        Character = character,
                        Server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id)
                    };
                    player.DefaultCharacters.Add(defChar);
                }
                else
                {
                    defChar.Character = character;
                }
                ctx.SaveChanges();
                await ReplyAsync(Localization.Format(server.Localization, "character_default_set", character.Name));
            }
            catch (Exception e)
            {
                await ReplyAsync(Localization.Format(server.Localization, "character_default_failed", nameOrAlias, e.Message));
                Console.WriteLine($"Couldn't set '{nameOrAlias}' as default character: {e.Message}\n{e.StackTrace}");
            }
        }

        [Command("setPrivateChannel")]
        [Alias("private")]
        [Summary("Defines the current channel as your private one, where you'll be able to perform private operations, like displaying the full details of your characters.")]
        public async Task SetPrivateChannel()
        {
            using var ctx = new AvatarBotContext();
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                //player.LoadPrivateChannels(ctx);
                var privateChannel = player.PrivateChannels.SingleOrDefault(x => x.Server.DiscordID == (long)Context.Guild.Id);
                if (privateChannel == null)
                    privateChannel = new Model.Servers.PrivateChannel()
                    {
                        Player = player,
                        ChannelDiscordID = (long)Context.Channel.Id,
                        Server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id)
                    };
                else
                    privateChannel.ChannelDiscordID = (long)Context.Channel.Id;
                ctx.SaveChanges();
                await ReplyAsync(Localization.Format(server.Localization, "private_channel_set", Context.Channel.Name));
            }
            catch (Exception e)
            {
                await ReplyAsync(Localization.Format(server.Localization, "private_channel_failed", Context.Channel.Name, e.Message));
                Console.WriteLine($"Couldn't set #{Context.Channel.Name} as your private channel: {e.Message}\n{e.StackTrace}");
            }
        }

        [Command("setPrivateChannel")]
        [Alias("private")]
        [Summary("Defines the current channel as your private one, where you'll be able to perform private operations, like displaying the full details of your characters.")]
        public async Task SetPrivateChannel([Summary("\nThe channel to define as your private channel.")] ISocketMessageChannel chan)
        {
            using var ctx = new AvatarBotContext();
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                player.LoadPrivateChannels(ctx);
                var privateChannel = player.PrivateChannels.SingleOrDefault(x => x.Server.DiscordID == (long)Context.Guild.Id);
                if (privateChannel == null)
                    privateChannel = new Model.Servers.PrivateChannel()
                    {
                        Player = player,
                        ChannelDiscordID = (long)chan.Id,
                        Server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id)
                    };
                else
                    privateChannel.ChannelDiscordID = (long)chan.Id;
                ctx.SaveChanges();
                await ReplyAsync(Localization.Format(server.Localization, "private_channel_set", chan.Name));
            }
            catch (Exception e)
            {
                await ReplyAsync(Localization.Format(server.Localization, "private_channel_failed", chan.Name, e.Message));
                Console.WriteLine($"Couldn't set #{chan.Name} as your private channel: {e.Message}\n{e.StackTrace}");
            }
        }

        [Command("verbose")]
        [Alias("t")]
        [Summary("Changes the verbose state of the bot (on verbose, it will send you back every command it receives)")]
        public async Task SetVerbose()
        {
            using var ctx = new AvatarBotContext();
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                player.Verbose = !player.Verbose;
                ctx.SaveChanges();

                await ReplyAsync(Localization.Format(server.Localization, "bot_verbose", player.Verbose ? "Verbose" : "Silent"));
            }
            catch (Exception e)
            {
                await ReplyAsync(Localization.Format(server.Localization, "bot_verbose_error", e.Message));
                Console.WriteLine($"Couldn't change verbose status: {e.Message}\n{e.StackTrace}");
            }
        }

        //[Command("password")]
        //[Alias("t")]
        //[Summary("Resets the password to login on the website")]
        //public async Task GenerateToken()
        //{
        //    using (var ctx = new AvatarBotContext())
        //    {
        //        try
        //        {
        //            var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
        //            var newPassword = player.GenerateToken();
        //            ctx.SaveChanges();

        //            await Context.User.SendMessageAsync($"Your new password is: {newPassword}");
        //        }
        //        catch (Exception e)
        //        {
        //            await ReplyAsync($"Couldn't generate token: {e.Message}");
        //            Console.WriteLine($"Couldn't generate token: {e.Message}\n{e.StackTrace}");
        //        }
        //    }
        //}
    }
}
