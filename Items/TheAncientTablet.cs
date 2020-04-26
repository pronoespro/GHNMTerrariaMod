using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    [AutoloadEquip(EquipType.Shield)]
    class TheAncientTablet : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The ancient tablet");
            Tooltip.SetDefault("The tablet that has the words of the gods " +
                "\nNo fall damage" +
                "\n+1 meele speed" +
                "\n+3 magic damage" +
                "\nDouble dark damage" +
                "\nquintuple jump, high jump and no knockback" +
                "\nInmune to on fire, cold and walk on water" +
                "\nmax minions +6" +
                "\n+200 mana");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 14;
            item.value = 10;
            item.defense = 75;
            item.rare = 2;
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 100);
            recipe.AddIngredient(mod.ItemType("AncientTablet"));
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(mod.GetTile("Altar"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            DarkPlayer dp = player.GetModPlayer<DarkPlayer>();

            dp.darknessRecovery += 3;
            dp.MaxDarkness += 50;

            DarkWorld dw = ModContent.GetInstance<DarkWorld>();

            if (NPC.downedMoonlord)
            {
                item.defense = 100;
            }
            if (dw.downedGodessQueen)
            {
                item.defense = 150;
            }
            DarkPlayer Dplayer = player.GetModPlayer<DarkPlayer>();
            player.noFallDmg = true;
            player.meleeSpeed += 1;
            player.maxMinions += 6;
            player.magicDamage += 3;
            player.statManaMax2 += 200;
            player.doubleJumpBlizzard = true;
            player.doubleJumpCloud = true;
            player.doubleJumpFart = true;
            player.doubleJumpSail = true;
            player.doubleJumpSandstorm = true;
            player.doubleJumpUnicorn = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.waterWalk = true;
            player.jumpBoost = true;
            player.noKnockback = true;
            Dplayer.doubleDarkDamage = true;
        }

    }
}
