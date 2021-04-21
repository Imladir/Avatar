using Discord;
using Discord.Commands;
using AvatarBot.Bot.Tools;
using AvatarBot.Model;
using AvatarBot.Model.Identity;
using AvatarBot.Model.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvatarBot.Bot.Modules
{
    [Group("admin")]
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Group("gm")]
        public class GmModule : ModuleBase<SocketCommandContext>
        {
            [RequireOwner(Group = "Permission")]
            [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
            [Command("add")]
            [Alias("a")]
            [Summary("Gives an user GM rights over the bot. Only the server's owner can use that command.")]
            public async Task AddGM([Summary("User to grant GM rights to.")]
                                    IUser user)
            {
                try
                {
                    var newGM = user as IGuildUser;

                    using var ctx = new AvatarBotContext();
                    var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                    if (server.IsGM(ctx, newGM.Id)) throw new Exception(Localization.Format(server.Localization, "gm_already", newGM.Nickname));

                    var player = ctx.Users.SingleOrDefault(x => x.DiscordID == (long)newGM.Id);
                    if (player == null)
                        player = new User() { DiscordID = (long)newGM.Id };

                    player.Roles.Add(new UserRole() { User = player, Role = ctx.Roles.Single(x => x.Name.Equals("GM")), Server = server });
                    player.Claims.Add(new UserClaim() { User = player, ClaimType = $"Server-{server.DiscordID}", ClaimValue = "GM" });
                    ctx.SaveChanges();
                    await ReplyAsync(Localization.Format(server.Localization, "gm_added", newGM.Nickname));

                }
                catch (Exception e)
                {
                    await ReplyAsync(e.Message);
                    Console.WriteLine($"Exception: {e.Message}:\n{e.StackTrace}");
                }
            }

            [RequireOwner(Group = "Permission")]
            [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
            [Command("remove")]
            [Alias("r")]
            [Summary("Removes an user's GM rights over the bot. Only the server's owner can use that command.")]
            public async Task RemoveGM([Summary("User to remove GM rights to.")]
                                    IUser user)
            {
                try
                {
                    var gm = user as IGuildUser;

                    using (var ctx = new AvatarBotContext())
                    {
                        var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                        if (!server.IsGM(ctx, gm.Id)) throw new Exception(Localization.Format(server.Localization, "gm_not", gm.Nickname));

                        var player = ctx.Users.SingleOrDefault(x => x.DiscordID == (long)gm.Id);

                        var roleQuery = from ur in ctx.UserRoles
                                        join u in ctx.Users on ur.User.ID equals u.ID
                                        join r in ctx.Roles on ur.Role.ID equals r.ID
                                        where ur.Server.ID == server.ID && u.ID == player.ID && r.Name.Equals("GM")
                                        select ur;

                        player.Roles.Remove(roleQuery.Single());
                        var claim = player.Claims.Single(x => x.ClaimType.Equals($"Server-{server.DiscordID}") && x.ClaimValue.Equals(roleQuery.Single().Role.Name));
                        player.Claims.Remove(claim);
                        ctx.SaveChanges();
                        await ReplyAsync(Localization.Format(server.Localization, "gm_removed", gm.Nickname));
                    }
                }
                catch (Exception e)
                {
                    await ReplyAsync(e.Message);
                    Console.WriteLine($"Exception: {e.Message}:\n{e.StackTrace}");
                }
            }
        }

        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [Command("setlocalization")]
        [Summary("Changes the localization for the bot's messages")]
        public async Task ChangeLocalization([Summary("New prefix for the bot's commands")]
                                             string local)
        {
            using (var ctx = new AvatarBotContext())
            {
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                try
                {
                    local = local.ToLower();
                    if (local.Length != 2)
                        throw new Exception(Localization.Format(server.Localization, "local_too_long", local));

                    server.Localization = local;
                    ctx.SaveChanges();
                    await ReplyAsync(Localization.Format(server.Localization, "local_changed", local));
                }
                catch (Exception e)
                {
                    await ReplyAsync(e.Message);
                    Console.WriteLine($"Exception: {e.Message}:\n{e.StackTrace}");
                }
            }
        }

        [RequireOwner(Group = "Permission")]
        [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
        [Command("setprefix")]
        [Summary("Changes the prefix for the bot's commands")]
        public async Task ChangePrefix([Summary("New prefix for the bot's commands")]
                                    char prefix)
        {
            using (var ctx = new AvatarBotContext())
            {
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                try
                {
                    if (prefix == '@' || prefix == '#')
                        throw new Exception(Localization.Format(server.Localization, "prefix_failed", prefix));

                    server.Prefix = $"{prefix}";
                    ctx.SaveChanges();
                    await ReplyAsync(Localization.Format(server.Localization, "prefix_changed", prefix));
                }
                catch (Exception e)
                {
                    await ReplyAsync(e.Message);
                    Console.WriteLine($"Exception: {e.Message}:\n{e.StackTrace}");
                }
            }
        }
    }
}
