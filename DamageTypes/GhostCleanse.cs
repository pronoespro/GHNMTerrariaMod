using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.DamageTypes
{
    public abstract class GhostCleanse : ModItem
    {

        public static bool cleanseDamage = true;

        public override void SetDefaults()
        {
            item.melee = false;
            item.magic = false;
            item.ranged = false;
            item.thrown = false;
            item.summon = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var tt = tooltips.FirstOrDefault(x => x.Name == "Damage " && x.mod == "Terraria");
            if (tt != null)
            {
                string[] split = tt.text.Split(' ');
                tt.text = split.First() + " cleansing" + split.Last();
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            DarkPlayer Dplayer = player.GetModPlayer<DarkPlayer>();
            mult = mult + Dplayer.CleansingDamage;

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

    }
}
