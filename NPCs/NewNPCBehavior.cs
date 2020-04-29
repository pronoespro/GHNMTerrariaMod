using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.NPCs
{
    class NewNPCBehavior:GlobalNPC
    {

        public override bool CheckDead(NPC npc)
        {
            DarkPlayer dp = Main.player[Main.myPlayer].GetModPlayer<DarkPlayer>();
            dp.RecoverDarkness(dp.darknessRecovery);
            return base.CheckDead(npc);
        }

    }
}
