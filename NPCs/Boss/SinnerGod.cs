using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.NPCs.Boss
{
    [AutoloadBossHead]
    public class SinnerGod : ModNPC
    {

        private Player player;
        private float speed;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sinner god");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 33333;
            npc.damage = 50;
            npc.defense = 3;
            npc.knockBackResist = 0;
            npc.width = 164;
            npc.height = 164;
            npc.scale = 1.25f;
            npc.value = 10000;
            npc.npcSlots = 1;
            npc.boss = true;
            npc.lavaImmune = false;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit15;
            npc.DeathSound = SoundID.NPCDeath15;
            music = MusicID.Boss5;
            bossBag = mod.ItemType("SinnerGodBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.1f * bossLifeScale);
            npc.damage = (int)(npc.damage * 1.55f);
            npc.defense = (int)(npc.defense * numPlayers * 0.75f);
        }

        private void Target()
        {
            player = Main.player[npc.target];
        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

        private void Move(Vector2 offset)
        {
            if (npc.life < npc.lifeMax / 2)
            {
                speed = 50;
            }
            else
            {
                speed = 20f;
            }
            Vector2 moveTo = player.Center + offset;
            Vector2 move = moveTo - npc.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                magnitude *= speed / magnitude;
            }
            float turnResistance = 3f;
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                magnitude *= speed / magnitude;
            }
            move.Normalize();
            npc.velocity = move * magnitude;
        }

        private void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2((0f + npc.velocity.X) / 3f, (-10f + npc.velocity.Y) / 3);
                    if (npc.timeLeft > 10 || npc.timeLeft < 0)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
        }

        private void Shoot(Vector2 position)
        {
            int type = mod.ProjectileType("GodBlades");
            Vector2 velocity = player.Center - (npc.Center + position);
            float magnitude = Magnitude(velocity);
            if (magnitude > 0)
            {
                velocity *= 10f / magnitude;
            }
            else
            {
                velocity = new Vector2(0, 10);
            }
            Projectile.NewProjectile(npc.Center + position, velocity, type, (int)(npc.damage * 0.75f), 5f);
            npc.ai[1] = 100 + Main.rand.Next(50);
        }

        private void FirstAttack()
        {
            Shoot(Vector2.Zero);
        }

        private void SecondAttack()
        {
            Shoot(new Vector2(0, 50));
            Shoot(new Vector2(0, -50));
            Shoot(new Vector2(50, 0));
            Shoot(new Vector2(-50, 0));
        }

        private void ThirdAttack()
        {
            Shoot(new Vector2(150, 150));
            Shoot(new Vector2(150, -150));
            Shoot(new Vector2(-150, 150));
            Shoot(new Vector2(-150, -150));

            Shoot(new Vector2(200, 0));
            Shoot(new Vector2(0, 200));
            Shoot(new Vector2(0, -200));
        }

        private void CoolPower()
        {
            npc.defense = 10;
            Vector2 dif = (player.Center - npc.Center);
            Vector2 offset = new Vector2(0, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(100, 0);
            Shoot(dif * 4 + offset);
            offset = new Vector2(-100, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(150, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(-150, 0);
            Shoot(dif * 2 + offset);
        }

        public override void AI()
        {
            Target();
            DespawnHandler();

            if (player.dead || !player.active) return;

            npc.ai[0] += (Main.expertMode) ? 3 : 1;
            if (npc.ai[0] < 500)
            {
                Move(new Vector2(150 + npc.width, 0));
            }
            else if (npc.ai[0] < 1000)
            {
                Move(new Vector2(0, -(150 + npc.height)));
            }
            else if (npc.ai[0] < 1500)
            {
                Move(new Vector2(-(150 + npc.width), 0));
            }
            else
            {
                if (npc.ai[0] > 2000)
                {
                    npc.ai[0] = 0;
                }
                Move(new Vector2(0, -(150 + npc.height)));
            }

            npc.ai[1] -= 1f;
            if (npc.ai[1] <= 0f)
            {
                FirstAttack();
                if (npc.life < npc.lifeMax * .75 && Main.expertMode)
                {
                    SecondAttack();
                }
                if (npc.life < npc.lifeMax / 2)
                {
                    ThirdAttack();
                }
                if (npc.life < npc.lifeMax / 4 && Main.expertMode)
                {
                    CoolPower();
                }
            }

        }

        private void RotateNPCToTarget()
        {
            if (player == null) return;
            Vector2 direction = npc.Center - player.Center;
            float rotation = (float)Math.Atan2(direction.Y, direction.X);
            float rotationtrimed = rotation + ((float)Math.PI * 0.5f);
            npc.rotation = (npc.rotation + rotationtrimed) / 2;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 2.0f);
            if (frame >= Main.npcFrameCount[npc.type])
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
            //RotateNPCToTarget();
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(5) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GodlyArmor"));
                }
                if (Main.rand.Next(4) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GodlyLeggings"));
                }
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GodlyHelmet"));
                }
                if (Main.rand.Next(2) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldShortswordOld);
                }
                if (Main.rand.Next(25) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DarkShadowCaster"));
                }
                if (Main.rand.Next(5) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowPunch"));
                }
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GodEssence"), 5 + Main.rand.Next(20));
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            float distance = (350 - Magnitude(player.position - position)) / 4;
            scale = 1.5f * (Math.Max(100, distance) / 100);
            return null;
        }

    }
}
