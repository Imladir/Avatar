using Discord;
using Discord.Commands;
using AvatarBot.Bot.Tools;
using AvatarBot.Model;
using AvatarBot.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvatarBot.Bot.Modules
{
    public class Characters : ModuleBase<SocketCommandContext>
    {

        [Command("create")]
        [Summary("Creates a new character")]
        public async Task CreateCharacter([Summary("Alias of the character to create")] string alias,
                                          [Summary("key=value pairs containing more informations about the character.\n" +
                                                   "If a value contains more than one word, put it between \"\".\n" +
                                                   "To learn more about the available fields, use the command **character fields**.")]
                                          [Remainder] CommandOptions<Character> options)
        {
            using var ctx = new AvatarBotContext();
            using var dbTransaction = ctx.Database.BeginTransaction();
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                var newChar = new PC()
                {
                    Alias = alias,
                    Name = alias,
                    Server = server,
                    Player = player
                };

                ctx.Characters.Add(newChar);
                // Users can't have hidden characters
                if (!ctx.HasPrivilege(server.ID, player.ID)) options.Params["hidden"] = "false";
                var msg = UpdateChar(ctx, newChar, options.Params);
                if (msg == "") msg = Localization.Format(server.Localization, "character_created", newChar.Name);
                ctx.SaveChanges();
                dbTransaction.Commit();
                await ReplyAsync(msg);
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                await ReplyAsync(Localization.Format(server.Localization, "character_creation_failed", e.Message));
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        [Command("update")]
        [Alias("u")]
        [Summary("Update the data of a character")]
        public async Task Update([Remainder]
                                 [Summary("key=value pairs containing more informations about the character.\n" +
                                                    "If a value contains more than one word, put it between \"\"." +
                                                    "To learn more about the available fields, use the command **character fields**.")]
                                 CommandOptions<Character> options)
        {
            using var ctx = new AvatarBotContext();
            using var dbTransaction = ctx.Database.BeginTransaction();
            try
            {
                // It's an update, I better load everything I can
                ctx.Characters.Attach(options.Target);
                //options.Target.FullLoad(ctx);
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);

                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                if (!ctx.HasPrivilege(server.ID, player.ID)) options.Params["hidden"] = "false";
                var msg = UpdateChar(ctx, options.Target, options.Params);
                if (msg == "") msg = $"{options.Target.Name} has been succesfully updated.";
                ctx.SaveChanges();
                dbTransaction.Commit();
                await ReplyAsync(msg);
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                await ReplyAsync(Localization.Format(server.Localization, "character_update_failed", e.Message));
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        private string UpdateChar(AvatarBotContext ctx, Character target, Dictionary<string, string> dic)
        {
            var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
            string msg = "";
            List<string> errors = new List<string>();

            foreach (var kv in dic)
            {
                if (!target.UpdateField(ctx, kv.Key, kv.Value)) errors.Add(Localization.Format(server.Localization, "property_set_error", kv.Key, kv.Value));
            }

            if (errors.Count != 0) msg = Localization.Format(server.Localization, "character_update_incomplete", target.Name, String.Join("\n", errors));
            return msg;
        }

        [Command("fields")]
        [Summary("Displays the available fields for character creation / update")]
        public async Task Fields()
        {
            string msg = $"Possible keys for characters creations and update are: " + String.Join(", ", PC.AcceptedFields) +
                         $".\nIn every command you can use more than one key=value pair. For example:\n" +
                         $"create satsume name=\"Doji Satsume\" color=green description=\"The Emerald Champion\"\n";
            await ReplyAsync(msg);
            await Context.Channel.DeleteMessageAsync(Context.Message);
        }
    }
}
