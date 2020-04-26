using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class Paper : ModItem
    {

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Paper");
            Tooltip.SetDefault("A normal pice of paper.");

        }

        public override void SetDefaults()
        {
            item.width = 15;
            item.height = 15;
            item.value = 10;
            item.rare = 2;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Wood, 1);
            recipe.SetResult(this, 10);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();

            recipe = new ModRecipe(mod);

            recipe.AddIngredient(this, 100);
            recipe.AddIngredient(ItemID.Leather, 10);
            recipe.SetResult(ItemID.Book, 10);

            recipe.AddTile(TileID.Tables);

            recipe.AddRecipe();

        }

    }
}
