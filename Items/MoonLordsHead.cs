using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    class MoonLordsHead : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moon Lord head");
            Tooltip.SetDefault("Proof of the end of the moon lord, loved by the gods" +
                "\nCan be used multiple times" +
                "\nNot the actual head of the moonlord" +
                "\nSummons the Queen of All");
        }

        public override void SetDefaults()
        {
            item.maxStack = 20;
            item.value = 1000;
            item.width = 50;
            item.height = 50;
            item.rare = 1;
            item.useAnimation = 40;
            item.useTime = 45;
            item.consumable = false;
            item.useStyle = 4;
        }

        public override bool CanUseItem(Player player)
        {

            DarkPlayer Dplayer = player.GetModPlayer<DarkPlayer>();

            bool QueenBee = NPC.downedQueenBee;

            bool bossOne = NPC.downedBoss1;
            bool bossTwo = NPC.downedBoss2;
            bool bossThree = NPC.downedBoss3;

            bool HardMode = Main.hardMode;

            bool mechOne = NPC.downedMechBoss1;
            bool mechTwo = NPC.downedMechBoss2;
            bool mechThree = NPC.downedMechBoss3;

            bool plantera = NPC.downedPlantBoss;
            bool golem = NPC.downedGolemBoss;
            bool cultist = NPC.downedAncientCultist;
            bool moonLord = NPC.downedMoonlord;

            bool day = Main.dayTime;
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("godessQueen"));

            return !alreadySpawned && moonLord && Dplayer.summonShadowMinion;
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("godessQueen"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.FragmentNebula, 15);
            recipe.AddIngredient(ItemID.FragmentSolar, 15);
            recipe.AddIngredient(ItemID.FragmentStardust, 15);
            recipe.AddIngredient(ItemID.FragmentVortex, 15);
            recipe.AddIngredient(ItemID.LunarBar, 5);

            recipe.AddTile(mod.TileType("Altar"));

            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
