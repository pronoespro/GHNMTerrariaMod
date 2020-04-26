using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Items.TreasureBags
{
    public class SinnerGodBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 666;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 9;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("GodlyArmor"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("GodlyLeggings"));
            }
            if (Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("GodlyHelmet"));
            }
            if (Main.rand.Next(3) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("DarkShadowCaster"), 1);
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("ShadowPunch"));
            }
            player.QuickSpawnItem(mod.ItemType("SinOfGod"), Main.rand.Next(3));
            player.QuickSpawnItem(mod.ItemType("GodEssence"), 10 + Main.rand.Next(40));
            player.QuickSpawnItem(ItemID.GoldCoin, 1 + Main.rand.Next(5));
        }

        public override int BossBagNPC => ModContent.NPCType<NPCs.Boss.SinnerGod>();

    }
}
