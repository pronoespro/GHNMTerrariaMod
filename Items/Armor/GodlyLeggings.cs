using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    class GodlyLeggings : ModItem
    {

        private bool ArmorSet = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Godly Leggings");
            Tooltip.SetDefault("A helmet to show status and power, no visible effects");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = 2;
            item.defense = 19;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            ArmorSet = (body.type == mod.ItemType("GodlyArmor") && head.type == mod.ItemType("GodlyHelmet"));
            return body.type == mod.ItemType("GodlyArmor") && head.type == mod.ItemType("GodlyHelmet");
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Archery, 5);
            if (!ArmorSet)
                player.AddBuff(BuffID.Blackout, 1);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GodEssence"), 10);
            recipe.AddTile(mod.TileType("Altar"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
