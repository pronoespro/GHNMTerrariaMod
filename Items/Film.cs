using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public abstract class Film : ModItem
    {

        public override void SetDefaults()
        {
            item.width = 15;
            item.height = 15;
            item.value = 10;
            item.rare = 2;
            item.maxStack = 999;
        }

    }
}
