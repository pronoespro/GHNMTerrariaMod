using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GodlyHemlet : ModItem
    {

        public bool ArmorSet = false;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Godly helmet");
            Tooltip.SetDefault("A helmet to show status and power, no visible effects");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = 2;
            item.defense = 17;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            ArmorSet = (body.type == mod.ItemType("GodlyArmor") && legs.type == mod.ItemType("GodlyLeggings"));
            return body.type == mod.ItemType("GodlyArmor") && legs.type == mod.ItemType("GodlyLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {

            player.buffImmune[BuffID.Blackout] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Confused] = true;
            /*
            bool coolDudecreated=false;
            foreach(Item item in player.inventory)
            {
                if (item.type == ItemID.Minishark && !coolDudecreated)
                {
                    coolDudecreated = true;
                    item.SetNameOverride("Cool dude 15");
                    item.prefix = PrefixID.Legendary;
                    item.damage = 222;
                    item.color = new Microsoft.Xna.Framework.Color(1, 0.6f, 0.6f);
                }
            }*/
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Calm, 5);
            if (!ArmorSet)
                player.AddBuff(BuffID.Cursed, 1);
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
