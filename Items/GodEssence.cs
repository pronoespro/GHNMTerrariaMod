using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class GodEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God essence");
            Tooltip.SetDefault("The essence of divinity");
        }

        public override void SetDefaults()
        {
            item.maxStack = 666;
            item.value = 10000;
            item.width = 32;
            item.height = 32;
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddIngredient(ItemID.Torch, 45);

            recipe.AddTile(mod.TileType("Altar"));

            recipe.SetResult(this, 5);

            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SinOfGod"));
            recipe.AddTile(mod.TileType("Altar"));

            recipe.SetResult(this, 5);

            recipe.AddRecipe();
        }
    }
}
