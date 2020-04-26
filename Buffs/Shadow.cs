using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Buffs
{
    class Shadow : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dark spirit");
            Description.SetDefault("Darkness protects you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            DarkPlayer DarkPlayer = player.GetModPlayer<DarkPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("ShadowGuardian")] > 0)
            {
                DarkPlayer.summonShadowMinion = true;
            }

            if (!DarkPlayer.summonShadowMinion)
            {
                player.DelBuff(buffIndex);
                DarkPlayer.ResetEffects();
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }

    }
}
