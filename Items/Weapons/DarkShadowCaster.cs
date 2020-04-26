using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Weapons
{
    public class DarkShadowCaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow releaser");
            Tooltip.SetDefault("Darkness gives strenght, \nbut watch your heart.");
        }

        public override void SetDefaults()
        {
            item.summon = true;
            item.maxStack = 1;
            item.autoReuse = false;
            item.mana = 6;
            item.damage = 150;
            item.width = 26;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = Item.buyPrice(0, 6, 6, 6);
            item.rare = -12;
            item.UseSound = SoundID.DD2_OgreRoar;
            item.shoot = mod.ProjectileType("ShadowGuardian");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("Shadow");
            item.buffTime = 30000;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {

            DarkPlayer DPlayer = player.GetModPlayer<DarkPlayer>();

            if (player.statMana < 6)
                return false;

            if (!DPlayer.summonShadowMinion)
            {
                item.buffType = mod.BuffType("Shadow");
                item.shoot = mod.ProjectileType("ShadowGuardian");
                DPlayer.summonShadowMinion = true;
            }
            else
            {
                item.buffType = -1;
                item.shoot = -1;
                DPlayer.summonShadowMinion = false;
                DPlayer.ResetEffects();
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 10);
            recipe.AddTile(mod.GetTile("Altar"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
