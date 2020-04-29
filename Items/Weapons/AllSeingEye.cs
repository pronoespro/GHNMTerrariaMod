using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Weapons
{
    class AllSeingEye : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("All seeing eye");
            Tooltip.SetDefault("Stare into everlasting Darkness" +
                "\nLeft click to teleport to nearest enemy and attack" +
                "\nright click to die?" +
                "\nleft click uses 15 darkness");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.damage = 125;
            item.useTime = 5;
            item.useAnimation = 5;
            item.width = 13;
            item.height = 13;
            item.scale = .25f;
            item.noMelee = false;
            item.magic = true;
            item.useStyle = 4;
            item.value = Item.buyPrice(gold: 5, silver: 25);
            item.rare = 13;
            item.UseSound = SoundID.NPCHit1;
        }

        public override bool UseItem(Player player)
        {
            DarkPlayer dplayer = player.GetModPlayer<DarkPlayer>();

            if (player.altFunctionUse == 2)
            {
                if (dplayer.CostDarkness(5, false))
                {
                    dplayer.ShadowGuardianAttack = 4;
                    dplayer.shadowGuardianDamage = Math.Max(1, player.statDefense / 10) * (item.damage / 10);
                    item.reuseDelay = 25 + Math.Min(40, (2000 / Math.Max(1, player.statDefense)));
                    item.autoReuse = false;
                }
            }
            else
            {
                if (dplayer.CostDarkness(4, false))
                {
                    dplayer.ShadowGuardianAttack = 3;
                    dplayer.shadowGuardianDamage = item.damage;
                    item.reuseDelay = 15;
                    item.autoReuse = true;
                }
            }

            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 200);
            recipe.AddTile(mod.GetTile("Altar"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

    }
}
