﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvatarBot.Bot.Properties {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringsEN {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringsEN() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AvatarBot.Bot.Properties.StringsEN", typeof(StringsEN).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Characters:.
        /// </summary>
        internal static string allcharacters_header {
            get {
                return ResourceManager.GetString("allcharacters_header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Characters matching filter &apos;{0}&apos;.
        /// </summary>
        internal static string allcharacters_header_filter {
            get {
                return ResourceManager.GetString("allcharacters_header_filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The bot verbose status (for you only) has been set to {0}..
        /// </summary>
        internal static string bot_verbose {
            get {
                return ResourceManager.GetString("bot_verbose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t change verbose status: {0}..
        /// </summary>
        internal static string bot_verbose_error {
            get {
                return ResourceManager.GetString("bot_verbose_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à {0} has been succesfully created..
        /// </summary>
        internal static string character_created {
            get {
                return ResourceManager.GetString("character_created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t create the character: {0}..
        /// </summary>
        internal static string character_creation_failed {
            get {
                return ResourceManager.GetString("character_creation_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t set &apos;{0}&apos; as default character: {1}..
        /// </summary>
        internal static string character_default_failed {
            get {
                return ResourceManager.GetString("character_default_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à {0} has been set as your default character..
        /// </summary>
        internal static string character_default_set {
            get {
                return ResourceManager.GetString("character_default_set", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à No character found with name or alias &apos;{0}&apos;..
        /// </summary>
        internal static string character_not_found {
            get {
                return ResourceManager.GetString("character_not_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t update the character: {0}..
        /// </summary>
        internal static string character_update_failed {
            get {
                return ResourceManager.GetString("character_update_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Some errors occured while updating {0}:\n{1}\nThe rest (if anything) was saved..
        /// </summary>
        internal static string character_update_incomplete {
            get {
                return ResourceManager.GetString("character_update_incomplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Default characters:.
        /// </summary>
        internal static string characters_header {
            get {
                return ResourceManager.GetString("characters_header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Default characters matching filter &apos;{0}&apos;.
        /// </summary>
        internal static string characters_header_filter {
            get {
                return ResourceManager.GetString("characters_header_filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Help for command **{0}{1}**\n\nAliases:.
        /// </summary>
        internal static string command_help {
            get {
                return ResourceManager.GetString("command_help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Sorry, I couldn&apos;t find a command like **{0}**..
        /// </summary>
        internal static string command_not_found {
            get {
                return ResourceManager.GetString("command_not_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Parameters: {}: {}\n.
        /// </summary>
        internal static string command_parameters {
            get {
                return ResourceManager.GetString("command_parameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Summary: {0}\n\n.
        /// </summary>
        internal static string command_summary {
            get {
                return ResourceManager.GetString("command_summary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à &apos;{0}&apos; is now a GM on the server..
        /// </summary>
        internal static string gm_add {
            get {
                return ResourceManager.GetString("gm_add", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à &apos;{0}&apos; is already a GM on the server..
        /// </summary>
        internal static string gm_already {
            get {
                return ResourceManager.GetString("gm_already", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à &apos;{0}&apos; is not a GM on the server..
        /// </summary>
        internal static string gm_not {
            get {
                return ResourceManager.GetString("gm_not", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à &apos;{0}&apos; is not a GM on the server anymore..
        /// </summary>
        internal static string gm_removed {
            get {
                return ResourceManager.GetString("gm_removed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Commands available for the bot. To get more details about command *cmd*, type **{0}help cmd**..
        /// </summary>
        internal static string help_header {
            get {
                return ResourceManager.GetString("help_header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Failed to retrieve characters list: {0}..
        /// </summary>
        internal static string list_failed {
            get {
                return ResourceManager.GetString("list_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Localization changed to &apos;{0}&apos;..
        /// </summary>
        internal static string local_changed {
            get {
                return ResourceManager.GetString("local_changed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Failed to change localization to &apos;{0}&apos;: code must be two characters long..
        /// </summary>
        internal static string local_too_long {
            get {
                return ResourceManager.GetString("local_too_long", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t edit message: {0}..
        /// </summary>
        internal static string message_edit_error {
            get {
                return ResourceManager.GetString("message_edit_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t send message: {0}..
        /// </summary>
        internal static string message_send_error {
            get {
                return ResourceManager.GetString("message_send_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Characters belonging to you:.
        /// </summary>
        internal static string mycharacters_header {
            get {
                return ResourceManager.GetString("mycharacters_header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Characters belonging to you maching filter &apos;{0}&apos;:.
        /// </summary>
        internal static string mycharacters_header_filter {
            get {
                return ResourceManager.GetString("mycharacters_header_filter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à No character found..
        /// </summary>
        internal static string no_character_found {
            get {
                return ResourceManager.GetString("no_character_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Prefix has been changed to &apos;{0}&apos;..
        /// </summary>
        internal static string prefix_changed {
            get {
                return ResourceManager.GetString("prefix_changed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Failed to change prefix to &apos;{0}&apos;: too many possible conflicts..
        /// </summary>
        internal static string prefix_failed {
            get {
                return ResourceManager.GetString("prefix_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t set #{0} as your private channel: {1}..
        /// </summary>
        internal static string private_channel_failed {
            get {
                return ResourceManager.GetString("private_channel_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à #{0} has been set as your private channel..
        /// </summary>
        internal static string private_channel_set {
            get {
                return ResourceManager.GetString("private_channel_set", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Couldn&apos;t set &apos;{0}&apos; with value &apos;{1}&apos;..
        /// </summary>
        internal static string property_set_error {
            get {
                return ResourceManager.GetString("property_set_error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Unknown command &apos;{0}&apos;..
        /// </summary>
        internal static string unknown_command {
            get {
                return ResourceManager.GetString("unknown_command", resourceCulture);
            }
        }
    }
}