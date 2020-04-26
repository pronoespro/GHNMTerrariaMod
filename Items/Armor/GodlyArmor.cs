using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GodlyArmor : ModItem
    {

        private bool ArmorSet = false;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Godly Chestplate");
            Tooltip.SetDefault("A chestplate to show status and power, no visible effects");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = 2;
            item.defense = 25;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            ArmorSet = (legs.type == mod.ItemType("GodlyLeggings"));
            return legs.type == mod.ItemType("GodlyLeggings");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Shine, 5);
            if (!ArmorSet)
                player.AddBuff(BuffID.Confused, 1);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 15);
            recipe.AddTile(mod.TileType("Altar"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
}
