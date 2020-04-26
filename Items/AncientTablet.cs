using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    [AutoloadEquip(EquipType.Shield)]
    class AncientTablet : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient tablet");
            Tooltip.SetDefault("'A tablet that gives you many powers'" +
                "\n+.25 meele speed" +
                "\ntriple jump and high jump" +
                "\nInmune to on fire and fall damage, and walk on water" +
                "\nmax minions +2 and +10 defense every preharmode boss defeated" +
                "\n+1 jump every preharmode boss defeated");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 14;
            item.value = 10;
            item.defense = 10;
            item.rare = 2;
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 75);
            recipe.AddTile(mod.GetTile("Altar"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            DarkPlayer dp = player.GetModPlayer<DarkPlayer>();

            dp.darknessRecovery += 1;

            player.doubleJumpBlizzard = true;
            player.doubleJumpCloud = true;
            if (NPC.downedBoss1)
            {
                item.defense = 20;
                player.doubleJumpFart = true;
                if (NPC.downedBoss2)
                {
                    item.defense = 30;
                    player.doubleJumpSail = true;
                    if (NPC.downedBoss3)
                    {
                        item.defense = 40;
                        player.doubleJumpSandstorm = true;
                        if (NPC.downedPlantBoss)
                        {
                            item.defense = 50;
                            player.doubleJumpUnicorn = true;
                        }
                    }
                }
            }
            player.noFallDmg = true;
            player.meleeSpeed += 0.25f;
            player.maxMinions += 2;
            player.magicDamage += 1.5f;
            player.buffImmune[BuffID.OnFire] = true;
            player.waterWalk = true;
            player.jumpBoost = true;
        }

    }
}
