using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class DarkOrb:ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Orb");
            Tooltip.SetDefault("A sphere containing darkness \nRestores 20 Darkness");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 99;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item3;
            item.value = Item.buyPrice(copper:50);
            item.rare = ItemRarityID.Blue;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("GodEssence"), 20);
            recipe.AddTile(mod.GetTile("Altar"));
            recipe.SetResult(this,5);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            DarkPlayer dp = player.GetModPlayer<DarkPlayer>();
            return dp.RecoverDarkness(20);
        }

    }
}
