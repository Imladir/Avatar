using AvatarBot.Model.Identity;
using AvatarBot.Model.Servers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarBot.Model.Characters
{
    [Table("PlayerCharacters")]
    public class PC : Character
    {
        [NotMapped]
        public static new readonly List<string> AcceptedFields = new List<string>() { "description" }.Concat(Character.AcceptedFields).OrderBy(s => s).ToList();

        public virtual User Player { get; set; }

        public override void FullLoad(AvatarBotContext ctx)
        {
            ctx.Entry(this).State = EntityState.Deleted;
            base.FullLoad(ctx);
            LoadPlayer(ctx);
        }

        public void LoadPlayer(AvatarBotContext ctx)
        {
            ctx.Entry(this).Reference(x => x.Player).Load();
        }

        public override void Update(AvatarBotContext ctx, Dictionary<string, string> args)
        {
            foreach (var kv in args)
                if (!base.UpdateField(ctx, kv.Key, kv.Value)) UpdateField(ctx, kv.Key, kv.Value);
        }
    }
}
