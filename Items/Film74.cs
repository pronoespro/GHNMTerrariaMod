using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    class Film74 : Film
    {
        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Type-74 Film");
            Tooltip.SetDefault("A camera film with ghost exorcism power" +
                "\nGreat exorcism power");

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("Paper"), 15);
            recipe.AddIngredient(ItemID.SoulofLight);
            recipe.AddIngredient(ItemID.SoulofNight);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }
    }
}
