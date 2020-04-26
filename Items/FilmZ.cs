using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class FilmZ : Film
    {

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Type-Zero Film");
            Tooltip.SetDefault("A camera film with ghost exorcism power" +
                "\nIncredible exorcism power");

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("Paper"), 25);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }

    }
}
