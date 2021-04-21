using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace AvatarBot.Bot.Tools
{
    class Localization
    {
        public static Dictionary<string, ResourceSet> Strings { get; set; } = new Dictionary<string, ResourceSet>();

        /** Load the resource files into the dictionary */
        public static void LoadData()
        {
            Strings["en"] = Properties.StringsEN.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            Strings["fr"] = Properties.StringsFR.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        }

        /** Finds the required string in the resources then formats its with the given arguments */
        public static string Format(string language, string name, params object[] args)
        {
            try
            {
                var str = Strings[language].GetString(name);
                if (str == null || str == "")
                {
                    if (language != "en") return Format("en", name, args);
                    else throw new Exception($"unknown string '{name}'");
                }
                else
                {
                    return String.Format(str, args);
                }
            } catch (Exception)
            {
                if (language != "en") return Format("en", name, args);
                else throw new Exception($"unknown string '{name}'");
            }
        }
    }
}
