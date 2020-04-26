using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    class Film37 : Film
    {

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Type-37 Film");
            Tooltip.SetDefault("A camera film with ghost exorcism power" +
                "\nHigh exorcism power");

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("Paper"), 10);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }
    }
}
