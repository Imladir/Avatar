using Discord;
using Discord.Commands;
using EmeraldBot.Bot.Displays;
using EmeraldBot.Bot.Tools;
using EmeraldBot.Model;
using EmeraldBot.Model.Characters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmeraldBot.Bot.Modules
{
    public class PrintAndList : ModuleBase<SocketCommandContext>
    {
        [Command("list")]
        [Alias("l")]
        [Summary("Displays a list of characters, techniques or skills")]
        public async Task PrintList([Summary("The first required parameter is the type of things you want to display. " +
                                             "It must be one of MyCharacters, Characters, AllCharacters, Skills or Techniques.")]
                                    string type,
                                    [Remainder]
                                    [Summary("You can filter those results by adding something after that, for example: " +
                                             "**!list techniques soul**.\n\n" +
                                             "Note that in the case of techniques, the filter is **required** (too many things to display at once otherwise).")]
                                    string filter = "")
        {
            filter = filter.ToLower().Trim();
            using var ctx = new AvatarBotContext();
            try
            {
                List<string> msg;

                type = type.ToLower();
                switch (type)
                {
                    case "mycharacters": msg = PrintListCharacters(filter, 0); break;
                    case "characters": msg = PrintListCharacters(filter, 1); break;
                    case "allcharacters": msg = PrintListCharacters(filter, 2); break;
                    default: throw new System.Exception($"Unknown command '{type}'.");
                }
                if (msg.Count == 0)
                {
                    await ReplyAsync($"No character found.");
                }
                else
                {
                    foreach (var m in msg.FitDiscordMessageSize()) await ReplyAsync(m);
                }
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
                Console.WriteLine($"Exception: {e.Message}:\n{e.StackTrace}");
            }
        }

        public List<string> PrintListCharacters(string filter, int detailLevel)
        {
            List<string> msg = new List<string>();

            using (var ctx = new AvatarBotContext())
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                List<PC> pcs = ctx.Characters.OfType<PC>().Where(x => x.Server.DiscordID == (long)Context.Guild.Id
                                                          && (filter.Equals("") || x.Name.Contains(filter) || x.Alias.Contains(filter))
                                                          && (x.Hidden == false || ctx.HasPrivilege(server.ID, player.ID))).ToList();

                foreach (var c in pcs.OrderBy(x => x.Name))
                {
                    if (detailLevel != 0 || c.Player.DiscordID == (long)Context.User.Id)
                    {
                        PC defPC = ctx.GetDefaultCharacter((ulong)c.Server.DiscordID, (ulong)c.Player.DiscordID);
                        bool def = defPC != null && defPC.ID == c.ID;
                        if (def || detailLevel != 1)
                            msg.Add(def ? $"**{c.Name}** (Alias: {c.Alias})" : $"{c.Name} (Alias: {c.Alias})");
                    }
                }
            }
            return msg;
        }

        [Command("print")]
        [Alias("p")]
        [Summary("Prints a character or technique")]
        public async Task Print([Summary("The alias of the character or technique to display.\n\n" +
                                         "If its a character you own and you're on your private channel, it will display everything, otherwise only the public profile.")]
                                string nameOrAlias,
                                string detailed = "")
        {
            ulong chanID = Context.Channel.Id;
            ulong serverID = Context.Guild.Id;
            var emds = new List<Embed>();

            using var ctx = new AvatarBotContext();
            try
            {
                var player = ctx.Users.Single(x => x.DiscordID == (long)Context.User.Id);
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                var c = ctx.Characters.SingleOrDefault(x => x.Server.DiscordID == (long)serverID
                                                         && (x.Name.Equals(nameOrAlias) || x.Alias.Equals(nameOrAlias))
                                                         && (x.Hidden == false || ctx.HasPrivilege(server.ID, player.ID)));
                if (c != null)
                {
                    if (c is PC pc)
                        emds.AddRange(pc.GetProfile(Context.Guild.Id, Context.User.Id, Context.Channel.Id, detailed.Equals("full", StringComparison.OrdinalIgnoreCase)));
                    else if (c is NPC npc)
                        emds.AddRange(npc.GetProfile(Context.Guild.Id, Context.User.Id, Context.Channel.Id, detailed.Equals("full", StringComparison.OrdinalIgnoreCase)));
                } else
                {
                    await ReplyAsync($"No character found with name or alias '{nameOrAlias}'");
                }

                foreach (var e in emds) await ReplyAsync("", false, e);
                await Context.Channel.DeleteMessageAsync(Context.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}: {e.StackTrace}");
                var inner = e.InnerException;
                while (inner != null)
                {
                    Console.WriteLine($"Inner Exception:\n{inner.Message}: {inner.StackTrace}");
                    inner = inner.InnerException;
                }
            }
        }
    }
}
