using Discord;
using Discord.Commands;
using AvatarBot.Bot.Tools;
using AvatarBot.Model;
using AvatarBot.Model.Characters;
using AvatarBot.Model.Identity;
using AvatarBot.Model.Servers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvatarBot.Bot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public Misc(CommandService service) { _service = service; }

        [Command("help")]
        [Summary("Shows what a command does and the parameters it needs.")]
        public async Task Help([Remainder] string command = "")
        {

            char prefix = ' ';
            string localization = "";
            using (var ctx = new AvatarBotContext())
            {
                var server = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id);
                prefix = server.Prefix[0];
                localization = server.Localization;
            }

            if (command == "")
            {
                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = Localization.Format(localization, "help_header", prefix)
                };

                foreach (var module in _service.Modules)
                {
                    string description = "";
                    foreach (var cmd in module.Commands)
                    {
                        var res = await cmd.CheckPreconditionsAsync(Context);
                        if (res.IsSuccess)
                            description += $"{prefix}{cmd.Aliases.First()}\n";
                    }

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        builder.AddField(x =>
                        {
                            x.Name = module.Name;
                            x.Value = description;
                            x.IsInline = false;
                        });
                    }
                }
                await ReplyAsync("", false, builder.Build());
            }
            else
            {
                var result = _service.Search(Context, command);
                if (!result.IsSuccess)
                {
                    await ReplyAsync(Localization.Format(localization, "command_not_found", command));
                    return;
                }

                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = Localization.Format(localization, "command_help", prefix, command)
                };

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;
                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Aliases);
                        x.Value =
                            Localization.Format(localization, "command_summary", cmd.Summary) +
                            Localization.Format(localization, "command_parameters", string.Join(", ", cmd.Parameters.Select(p => p.Name)), string.Join("", cmd.Parameters.Select(p => p.Summary)));
                        x.IsInline = false;
                    });
                }
                await ReplyAsync("", false, builder.Build());
            }
        }

        //[RequireOwner]
        //[Command("seed")]
        //[Summary("Seeds the database.")]
        //public async Task Seed()
        //{
        //    using var ctx = new AvatarBotContext();
        //    ctx.Seed();
        //    await ReplyAsync("Database has been seeded");
        //}
    }
}
