﻿using Discord;
using Discord.Commands;
using EmeraldBot.Bot.Tools;
using EmeraldBot.Model;
using EmeraldBot.Model.Characters;
using EmeraldBot.Model.Identity;
using EmeraldBot.Model.Servers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmeraldBot.Bot.Modules
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
            using (var ctx = new AvatarBotContext())
            {
                prefix = ctx.Servers.Single(x => x.DiscordID == (long)Context.Guild.Id).Prefix[0];
            }

            if (command == "")
            {
                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = "Commands available for the bot. To get more details about command *cmd*, type **!help cmd**"
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
                    await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                    return;
                }

                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = $"Help for command **{prefix}{command}**\n\nAliases: "
                };

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;
                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Aliases);
                        x.Value =
                            $"Summary: {cmd.Summary}\n\n" +
                            $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}: {string.Join("", cmd.Parameters.Select(p => p.Summary))}\n";
                        x.IsInline = false;
                    });
                }
                await ReplyAsync("", false, builder.Build());
            }
        }

        [RequireOwner]
        [Command("seed")]
        public async Task Seed()
        {
            using var ctx = new AvatarBotContext();
            ctx.Seed();
            await ReplyAsync("Database has been seeded");
        }
    }
}