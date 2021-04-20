using Discord;
using Discord.Commands;
using EmeraldBot.Bot.Tools;
using EmeraldBot.Model;
using EmeraldBot.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmeraldBot.Bot.Modules
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
            try
            {
                var newChar = new PC()
                {
                    Alias = alias,
                    Name = alias,
                    Server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id),
                    Player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id)
                };

                ctx.Characters.Add(newChar);
                var msg = UpdateChar(ctx, newChar, options.Params);
                if (msg == "") msg = $"{newChar.Name} has been succesfully created.";
                ctx.SaveChanges();
                dbTransaction.Commit();
                await ReplyAsync(msg);
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                dbTransaction.Rollback();
                await ReplyAsync($"Couldn't create the character: {e.Message}");
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
                options.Reattach(ctx);
                //options.Target.FullLoad(ctx);

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
                await ReplyAsync($"Couldn't update the character: {e.Message}");
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }

        private string UpdateChar(AvatarBotContext ctx, Character target, Dictionary<string, string> dic)
        {
            string msg = "";
            List<string> errors = new List<string>();

            foreach (var kv in dic)
            {
                if (!target.UpdateField(ctx, kv.Key, kv.Value)) errors.Add($"Couldn't set {kv.Key} with value {kv.Value}");
            }

            if (errors.Count != 0) msg = $"Some errors occured while updating {target.Name}:\n" + String.Join("\n", errors) + "\nThe rest (if anything) was saved.";
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
