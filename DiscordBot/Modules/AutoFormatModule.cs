using Discord;
using Discord.Commands;
using EmeraldBot.Bot.Tools;
using EmeraldBot.Model;
using EmeraldBot.Model.Characters;
using EmeraldBot.Model.Servers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmeraldBot.Bot.Modules
{
    public class AutoFormat : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Formats a message so that it can be displayed as in-character")]
        public async Task Say([Summary("The text to format, with an optional parameter or $alias")] [Remainder] CommandOptions<PC> options)
        {
            try
            {
                var msg = await Talk(options.Target, options.Text);
                using var ctx = new AvatarBotContext();
                ctx.Messages.Add(msg);
                msg.Server = ctx.Servers.SingleOrDefault(x => x.DiscordID == (long)Context.Guild.Id);
                msg.Player = ctx.Users.SingleOrDefault(x => x.DiscordID == (long)Context.User.Id);
                ctx.Entry(msg.Player).State = EntityState.Unchanged;
                ctx.Entry(msg.Server).State = EntityState.Unchanged;
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                await ReplyAsync($"Couldn't send message: {e.Message}");
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        [Command("npc")]
        [Summary("Creates a simple block of text for an NPC without creating an actual sheet. **Only the GM can use it.**")]
        public async Task NPCSay([Remainder]
                                  [Summary("By default, only needs the text to display. It will appear as if *A Passerby* had just talked.\n" +
                                           "If you want, you can give more details by supplying **key=value** pairs:\n" +
                                           "- **colour=xxx**: defines the color of the border.\n" +
                                           "- **name=xxx**: changes the name to xxx\n" +
                                           "- **icon=\"xxx.jpg\"**: must be a valid URL of an image (the actual image, not a page containing the image) and will display it instead of the clan mon.")]
                                  CommandOptions<Character> options)
        {
            using var ctx = new AvatarBotContext();
            try
            {
                var target = new PC();
                target.Update(ctx, options.Params);

                var msg = await Talk(target, options.Text);

                msg.Server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                msg.Player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                ctx.Messages.Add(msg);
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                await ReplyAsync($"Couldn't send message: {e.Message}");
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        private async Task<Message> Talk(PC target, string text)
        {
            var emd = AutoFormater.Format(target, text);
            var res = await Context.Channel.SendMessageAsync("", false, emd.Build());


            int colour = target.Colour;

            var message = new Message()
            {
                DiscordChannelID = (long)Context.Channel.Id,
                DiscordMessageID = (long)res.Id,
                LastUpdated = DateTime.UtcNow,
                Title = target.Name,
                Text = text,
                Colour = colour,
                Icon = target.Icon
            };


            await Context.Channel.DeleteMessageAsync(Context.Message);

            return message;
        }

        [Command("editlastmessage")]
        [Alias("edit")]
        [Summary("Edits the last message the bot formated for you.")]
        public async Task EditLastMessage([Remainder]
                                          [Summary("The new text replacing the old one.")]
                                          string data)
        {
            using var ctx = new AvatarBotContext();
            try
            {
                var message = ctx.Messages.Where(x => x.DiscordChannelID == (long)Context.Channel.Id
                                                   && x.Server.DiscordID == (long)Context.Guild.Id
                                                   && x.Player.DiscordID == (long)Context.User.Id)
                                          .OrderByDescending(x => x.LastUpdated).FirstOrDefault();

                if (message == null) return;

                message.Text = AutoFormater.SimpleFormat(data);

                IUserMessage msg = (IUserMessage)await Context.Channel.GetMessageAsync((ulong)message.DiscordMessageID);
                await msg.ModifyAsync(x => x.Embed = message.ToEmbed());
                message.LastUpdated = DateTime.UtcNow;
                ctx.SaveChanges();
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                await ReplyAsync($"Couldn't edit message: {e.Message}");
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        [Command("deletelastmessage")]
        [Summary("Deletes the last message the bot formated for you. If that message has already been deleted, this command will do nothing.")]
        public async Task DeleteLastMessage()
        {
            using (var ctx = new AvatarBotContext())
            {
                var message = ctx.Messages.Where(x => x.DiscordChannelID == (long)Context.Channel.Id
                                                   && x.Server.DiscordID == (long)Context.Guild.Id
                                                   && x.Player.DiscordID == (long)Context.User.Id)
                                          .OrderByDescending(x => x.LastUpdated).FirstOrDefault();
                if (message == null) return;

                IUserMessage msg = (IUserMessage)await Context.Channel.GetMessageAsync((ulong)message.DiscordMessageID);
                await msg.DeleteAsync();
                ctx.Messages.Remove(message);
                ctx.SaveChanges();
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
        }
    }
}
