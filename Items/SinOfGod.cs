using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items
{
    public class SinOfGod : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God's sin");
            Tooltip.SetDefault("The essence of sin." +
                "\nSummons the sinner god" + "\nwhose name was forgotten by the other gods");
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
            item.consumable = true;

            item.useStyle = 4;
        }

        public override bool CanUseItem(Player player)
        {
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
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("SinnerGod"));

            return !alreadySpawned && bossOne && bossTwo && bossThree && day;
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SinnerGod"));
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.CrimtaneOre, 10);
            recipe.AddIngredient(mod.ItemType("GodEssence"));

            recipe.AddTile(mod.TileType("Altar"));

            recipe.SetResult(this);

            recipe.AddRecipe();

            recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.DemoniteOre, 10);
            recipe.AddIngredient(mod.ItemType("GodEssence"));

            recipe.AddTile(mod.TileType("Altar"));

            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
