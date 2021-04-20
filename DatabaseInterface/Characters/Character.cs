using EmeraldBot.Model.Servers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmeraldBot.Model.Characters
{
    [Table("Characters")]
    public abstract class Character
    {
        [NotMapped]
        public static readonly List<string> AcceptedFields = new List<string>() { "name", "alias", "icon", "color", "description" }.OrderBy(s => s).ToList();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^\w{3,24}$", ErrorMessage = "Alias is not valid (needs to be a single word between 3 and 24 letters)")]
        public string Alias { get; set; }

        [Required]
        [MinLength(3), MaxLength(128)]
        public string Name { get; set; }
        [Required]
        public virtual Server Server { get; set; }
        public string Icon { get; set; } = "";
        public int Colour { get; set; } = 0;
        public string Description { get; set; } = "";

        public Character()
        {
            Alias = "";
            Name = "";
            Icon = "";
            Colour = 0;
            Description = "";
        }

        public virtual void FullLoad(AvatarBotContext ctx)
        {
            ctx.Entry(this).Reference(x => x.Server).Load();
        }

        public Character LoadServer(AvatarBotContext ctx)
        {
            ctx.Characters.Attach(this);
            ctx.Entry(this).Reference(x => x.Server).Load();
            return this;
        }

        public virtual void Update(AvatarBotContext ctx, Dictionary<string, string> args)
        {
            foreach (var kv in args) UpdateField(ctx, kv.Key, kv.Value);
        }

        public bool UpdateField(AvatarBotContext ctx, string field, string value)
        {
            try
            {
                switch (field.ToLower())
                {
                    case "name": Name = value; return true;
                    case "alias": Alias = value; return true;
                    case "description": Description = value; return true;
                    case "icon": Icon = value; return true;
                    case "colour": Colour = ParseColour(value); return true;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Caught an exception on update for {field} with value {value}:\n{e.Message}\n{e.StackTrace}");
                throw new Exception($"Couldn't update {field} with value {value}: {e.Message}");
            }
            return false;
        }

        private int ParseColour(string colour)
        {
            switch (colour.ToLower())
            {
                case "black": return 0x000000;
                case "red": return 16711680;
                case "lime": return 0x00FF00;
                case "blue": return 0x0000FF;
                case "yellow": return 0xFFFF00;
                case "cyan": return 0x00FFFF;
                case "magenta": return 0xFF00FF;
                case "silver": return 0xC0C0C0;
                case "gray": return 0x808080;
                case "maroon": return 0x800000;
                case "olive": return 0x808000;
                case "purple": return 0x800080;
                case "orange": return 0xFFA500;
                case "teal": return 0x008080;
                case "navy": return 0x000080;
                default: throw new Exception($"Unknown colour '{colour}'. Known colours are black, white, red, lime, blue, " +
                    $"yellow, cyan, magenta, silver, gray, maroon, olive, purple, orange, teal, navy");
            }
        }

        public static bool IsCharacter(AvatarBotContext ctx, string aliasOrName)
        {
            return ctx.Characters.Count(x => x.Name.Equals(aliasOrName) || x.Alias.Equals(aliasOrName)) > 0;
        }
    }
}
