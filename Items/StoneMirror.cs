using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class StoneMirror : ModItem
    {

        public override void SetStaticDefaults()
        {

            DisplayName.SetDefault("Stone Mirror");
            Tooltip.SetDefault("A polished, mirror-like stone." +
                "\n A protective spirit dwells inside." +
                "\nRevives you once" +
                "\nCan only use one every 3 minutes.");

        }

        public override void SetDefaults()
        {
            item.width = 15;
            item.height = 15;
            item.value = 10;
            item.rare = 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }

    }
}
