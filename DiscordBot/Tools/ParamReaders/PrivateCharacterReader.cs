using Discord.Commands;
using AvatarBot.Model;
using AvatarBot.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvatarBot.Bot.Tools.ParamReaders
{

    public class PrivateCharacterReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            try
            {
                using var ctx = new AvatarBotContext();
                var pc = new PrivateCharacter() { Character = ctx.GetPlayerCharacter(context.Guild.Id, context.User.Id, input.Replace("$", "")) };
                Console.WriteLine($"I found a matching private character: {pc.Character.Name}");
                return Task.FromResult(TypeReaderResult.FromSuccess(pc));
            }
            catch (Exception)
            {
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Could not find the private character."));
            }
        }
    }
}
