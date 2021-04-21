﻿using Discord;
using AvatarBot.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AvatarBot.Bot.Tools
{
    class AutoFormater
    {
        private static string speechPattern = "((\".*?\")|(“.*?”))";
        private static string thoughtsPattern = "\\~(.*?)\\~";

        public static EmbedBuilder Format(Model.Characters.PC character, string msg)
        {
            int colour = character.Colour;

            msg = SimpleFormat(msg);

            var emd = new EmbedBuilder();
            emd.WithTitle(character.Name);
            emd.WithDescription(msg);
            emd.WithColor(new Color((uint)colour));
            if (character.Icon != "") emd.WithThumbnailUrl(character.Icon);

            return emd;
        }

        public static string SimpleFormat(String msg)
        {
            msg = Regex.Replace(msg, speechPattern, SimpleSpeechFormater);
            msg = Regex.Replace(msg, thoughtsPattern, SimpleThoughtsFormater);

            return msg;
        }

        public static string MarkdownFormat(String msg)
        {
            msg = Regex.Replace(msg, speechPattern, MarkdownSpeechFormater);
            msg = Regex.Replace(msg, thoughtsPattern, MarkdownThoughtsFormater);

            return String.Format("```markdown\n{0}\n```", msg);
        }

        private static string SimpleSpeechFormater(Match match)
        {
            return String.Format("**{0}**", match.Value);
        }

        private static string SimpleThoughtsFormater(Match match)
        {
            return String.Format("*{0}*", match.Groups[1].Value);
        }

        private static string MarkdownSpeechFormater(Match match)
        {
            return String.Format("\n# {0}\n", match.Value);
        }

        private static string MarkdownThoughtsFormater(Match match)
        {
            return String.Format("< {0} >", match.Groups[1].Value);
        }
    }
}
