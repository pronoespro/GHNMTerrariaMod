using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class Film14 : Film
    {

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Type-14 Film");
            Tooltip.SetDefault("A camera film with ghost exorcism power" +
                "\nNormal exorcism power");

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("Paper"), 1);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }

    }
}
