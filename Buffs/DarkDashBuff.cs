using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Buffs
{
    class DarkDashBuff : ModBuff
    {

        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            DisplayName.SetDefault("Dark dash");
            Description.SetDefault("You are almost inmune to damage!");
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
            player.noFallDmg = true;
            player.noItems = true;
            player.statDefense += 1250;
            if (player.velocity.Length() < 10)
            {
                player.DelBuff(buffIndex);
            }
        }

    }
}
