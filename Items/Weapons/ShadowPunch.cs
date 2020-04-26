using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Weapons
{
    class ShadowPunch : ModItem
    {
        bool startWeaponLevel = true;
        int initialDamage = 20;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow punch");
            Tooltip.SetDefault("A dark fist that your guardian imitates" +
                "\nFires a dark punch from your shadow guardian" +
                "\nUses 1 darkness");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.autoReuse = true;
            item.reuseDelay = 1;
            item.useTime = 15;
            item.useAnimation = 15;
            item.width = 26;
            item.height = 26;
            item.noMelee = true;
            item.useStyle = 3;
            item.value = Item.buyPrice(gold: 1, silver: 25);
            item.rare = 13;
            item.UseSound = SoundID.NPCHit1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {

            item.damage = Math.Max(1, player.statDefense / 10) * initialDamage;
            DarkPlayer Dplayer = player.GetModPlayer<DarkPlayer>();


            if (Dplayer.doubleDarkDamage)
                item.damage *= 2;

            Dplayer.shadowGuardianDamage = item.damage;

            if (player.altFunctionUse == 2)
            {
                if (Dplayer.CostDarkness(4, true))
                {
                    Dplayer.shadowGuardianDamage *= Dplayer.darkDash.GetLevel();
                    Dplayer.ShadowGuardianAttack = 2;
                }
            }
            else
            {
                if (Dplayer.CostDarkness(1, false))
                {
                    Dplayer.shadowGuardianDamage = Dplayer.shadowGuardianDamage * (int)(0.75f + (Dplayer.shadowPunch.GetLevel() * 0.25f));
                    Dplayer.ShadowGuardianAttack = 1;
                }
            }


            return base.UseItem(player);
        }

    }
}
