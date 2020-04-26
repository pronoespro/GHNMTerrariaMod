using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    class AltarItem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Altar");
            Tooltip.SetDefault("Used for special crafting");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.scale = 1.25f;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 17;
            item.useTime = 140;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 1;
            item.createTile = mod.TileType("Altar");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.GoldBar, 3);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
