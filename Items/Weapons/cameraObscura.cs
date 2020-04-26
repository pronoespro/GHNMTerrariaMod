using GodsHaveNoMercy.DamageTypes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Weapons
{
    class cameraObscura : GhostCleanse
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Camera Obscura");
            Tooltip.SetDefault("A camera that can see what the naked eye can't" +
                "\nCan damage ghosts");
        }

        public override void SetDefaults()
        {

            item.damage = 20;
            item.noMelee = true;
            item.channel = true;
            item.mana = 2;
            item.rare = 5;
            item.width = 25;
            item.height = 25;
            item.useTime = 0;
            item.autoReuse = true;
            item.reuseDelay = 150;
            item.UseSound = SoundID.Item13;
            item.useStyle = 5;
            item.shootSpeed = 5;
            item.shoot = mod.ProjectileType("CameraFlash");
            item.value = Item.sellPrice(0, 1, 0, 0);

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddIngredient(ItemID.SoulofNight, 1);
            recipe.SetResult(this);

            recipe.AddTile(TileID.WorkBenches);

            recipe.AddRecipe();
        }

    }
}
