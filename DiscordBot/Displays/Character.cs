using Discord;
using Discord.Commands;
using AvatarBot.Bot.Tools;
using AvatarBot.Model;
using AvatarBot.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarBot.Bot.Displays
{
    public static class Display
    {
        public static List<Embed> GetProfile(this PC pc, ulong serverID, ulong userID, ulong chanID, bool fullDetails = false)
        {
            var emds = new List<Embed>();

            using var ctx = new AvatarBotContext();

            pc = ctx.PCs.Find(pc.ID);

            // Public data
            var emd = new EmbedBuilder();
            emd.WithTitle(pc.Name);
            emd.WithColor(new Color((uint)pc.Colour));
            emd.WithThumbnailUrl("https://gamepedia.cursecdn.com/l5r_gamepedia_en/thumb/1/1f/Rings.png/300px-Rings.png");
            if (pc.Icon != "") emd.WithThumbnailUrl(pc.Icon);
            if (pc.Description != "") emd.AddField("Description", pc.Description);

            emds.Add(emd.Build());

            return emds;
        }

        public static List<Embed> GetProfile(this NPC npc, ulong serverID, ulong userID, ulong chanID, bool fullDetails = false)
        {
            throw new NotImplementedException("Not done yet");
        }
    }
}
