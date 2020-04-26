using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Weapons
{
    class TerraBrand : ModItem
    {

        int atackType = 0;
        int atackTypeMax = 15;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Brand");
            Tooltip.SetDefault("A keychain attached to the keybrand" +
                "\nGives extra power to it");
        }

        public override void SetDefaults()
        {
            item.damage = 350;
            item.shoot = ProjectileID.SwordBeam;
            item.melee = true;
            item.shootSpeed = 25;
            item.maxStack = 1;
            item.useTime = 10;
            item.useAnimation = 10;
            item.width = 54;
            item.height = 54;
            item.scale = 1.5f;
            item.useStyle = 1;
            item.autoReuse = true;
            item.reuseDelay = 5;
        }

        public override void HoldItem(Player player)
        {
            Vector2 MousePos = new Vector2(Main.mouseX - (Main.screenWidth / 2 + player.Center.X), Main.mouseY - (Main.screenHeight / 2 + player.Center.Y));
            MousePos.Normalize();
            player.itemLocation = MousePos * item.width / 4 + player.Center;
            player.itemRotation = (MousePos.Y / MousePos.X) * MathHelper.Pi + 10;
            player.itemRotation += (player.itemRotation > 360) ? 360 : 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {

            if (player.altFunctionUse != 2)
            {
                item.noMelee = false;
                atackType += 1;
                if (atackType > atackTypeMax) atackType = 0;
                if (atackType == 0)
                {
                    item.shoot = ProjectileID.SwordBeam;
                    item.useStyle = 1;
                    item.useAnimation = 20;
                    item.useTime = 20;
                    item.reuseDelay = 30;
                    item.shootSpeed = 20;
                }
                else if (atackType == 1)
                {
                    item.shoot = ProjectileID.SwordBeam;
                    item.useStyle = 3;
                    item.useAnimation = 30;
                    item.useTime = 30;
                    item.reuseDelay = 40;
                    item.shootSpeed = 30;
                }
                else
                {
                    item.shoot = ProjectileID.Arkhalis;
                    item.useStyle = 5;
                    item.useAnimation = 50;
                    item.useTime = 50;
                    item.reuseDelay = 60;
                    item.shootSpeed = 5;
                }
            }
            else
            {
                item.noMelee = true;
                NPC target = new NPC();
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.friendly != true || npc.CanBeChasedBy(this, false))
                    {
                        if (npc.position != new Vector2() && Vector2.Distance(npc.position, player.position) < 400)
                        {
                            Vector2 relativePos = npc.position - player.position;
                            if (relativePos.Length() < (target.position - player.position).Length())
                                target = npc;
                        }
                    }
                }
                if (target.Distance(player.position) > 150)
                {
                    item.shoot = ProjectileID.IceBolt;
                    item.useStyle = 4;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.reuseDelay = 30;
                    item.shootSpeed = 20;
                }
                else
                {
                    item.shoot = ProjectileID.Fireball;
                    item.useStyle = 4;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.reuseDelay = 30;
                    item.shootSpeed = 3;
                }
            }

            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            /*
            recipe.AddIngredient(ItemID.TerraBlade);
            recipe.AddIngredient(ItemID.Keybrand);
            recipe.AddIngredient(ItemID.LifeCrystal,15);
            recipe.AddIngredient(ItemID.LifeFruit, 10);
            recipe.AddIngredient(ItemID.FallenStar, 50);
            */
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.SetResult(ItemID.LifeCrystal, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddRecipe();
        }

    }
}
