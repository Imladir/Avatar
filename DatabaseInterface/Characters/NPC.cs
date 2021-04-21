using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarBot.Model.Characters
{
    [Table("NonPlayerCharacters")]
    public class NPC : Character
    {
        [NotMapped]
        public static new readonly List<string> AcceptedFields = new List<string>() { }.Concat(Character.AcceptedFields).OrderBy(s => s).ToList();

        public NPC() : base()
        {
        }

        public override void FullLoad(AvatarBotContext ctx)
        {
            base.FullLoad(ctx);
        }

        public override void Update(AvatarBotContext ctx, Dictionary<string, string> args)
        {
            foreach (var kv in args) base.UpdateField(ctx, kv.Key, kv.Value);
        }
    }
}
