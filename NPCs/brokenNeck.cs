using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.NPCs
{
    class brokenNeck : ImmuneNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Neck");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 22;
            npc.lifeMax = 100;
            npc.damage = 50;
            npc.defense = 1;
            npc.knockBackResist = 1;
            npc.width = 38;
            npc.height = 64;
            npc.value = 200;
            npc.npcSlots = 2;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            isGhost = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void AI()
        {
            npc.immortal = false;
            base.AI();
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Film14"), Main.rand.Next(2));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            DarkPlayer dp = Main.player[Main.myPlayer].GetModPlayer<DarkPlayer>();
            return (spawnInfo.spawnTileY < Main.rockLayer && dp.ZoneHauntedMansion) ? 1f : 0f;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (CanBeHit(item))
            {
                npc.immortal = false;
            }
            else
            {
                knockback = 0;
                damage = 0;
                npc.immortal = true;
                return;
            }
            base.ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (CanBeHit(projectile))
            {
                npc.immortal = false;
            }
            else
            {
                knockback = 0;
                damage = 0;
                npc.immortal = true;
                npc.buffImmune[BuffID.OnFire] = true;
                return;
            }
            base.ModifyHitByProjectile(projectile, ref damage, ref knockback, ref crit, ref hitDirection);

        }

    }
}
