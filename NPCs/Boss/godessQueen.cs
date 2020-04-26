using GodsHaveNoMercy.DamageTypes;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.NPCs.Boss
{
    [AutoloadBossHead]
    class godessQueen : ModNPC
    {
        private Player player;
        private float speed;
        private bool trueForm;
        private bool firstTeleport;
        private float timeToNextFraseTeleport = 120;
        private int justStarted = 0;
        private float dustTimer = 5;

        private void LeaveDust()
        {
            if (--dustTimer <= 0)
            {
                Dust dust = Main.dust[Dust.NewDust(npc.Center, npc.width, npc.height, DustID.SolarFlare, Alpha: 50, SpeedX: npc.velocity.X / 2, SpeedY: npc.velocity.Y / 2)];
                dust.noLight = false;
                dustTimer = 5;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Godess Queen");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 666666;
            npc.damage = 75;
            npc.defense = 5;
            npc.knockBackResist = 0;
            npc.width = 164;
            npc.height = 164;
            npc.scale = 2f;
            npc.value = 3000000;
            npc.npcSlots = 100;
            npc.boss = true;
            npc.behindTiles = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit15;
            npc.DeathSound = SoundID.NPCDeath15;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/MotherOfAll");
            bossBag = mod.ItemType("SinnerGodBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * bossLifeScale * 0.5f);
            npc.defense = npc.defense * numPlayers;
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
                speed = 75;
            }
            else if (npc.life < npc.lifeMax / 4 * 3)
            {
                speed = 30;
            }
            else
            {
                speed = 10;
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

        private void CheckIfDamageIsCleansing()
        {
            Item selItem = player.inventory[player.selectedItem];
            ModItem modI = selItem.modItem;
            GhostCleanse GC = (GhostCleanse)modI;

            if (GC != null)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
        }

        private void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                if (justStarted != 4)
                {
                    Main.NewText("Hahaha, my son is avenged.", Color.DarkMagenta);
                    justStarted = 4;
                }
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

            if (npc.life < npc.lifeMax / 2)
            {
                npc.life -= npc.damage / 2;
            }

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
            npc.ai[1] = 100 + ((npc.life > npc.lifeMax * 0.75f) ? Main.rand.Next(50) : 50);
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
            Main.raining = true;
            Main.rainTime = 15;
            npc.defense = 0;
            npc.damage = 125;
            Vector2 dif = (player.Center - npc.Center);
            Vector2 offset = new Vector2(0, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(50, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(-50, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(100, 0);
            Shoot(dif * 2 + offset);
            offset = new Vector2(-100, 0);
            Shoot(dif * 2 + offset);

        }

        public override void AI()
        {
            LeaveDust();
            Target();
            DespawnHandler();

            timeToNextFraseTeleport--;

            double dist = Math.Sqrt(Math.Pow(player.position.X - npc.position.X, 2) + Math.Pow(player.position.Y - npc.position.Y, 2));

            if (justStarted == 0)
            {
                npc.ai[1] = 75 * 2f;
                justStarted = 1;
            }
            if (justStarted == 1 && npc.life < npc.lifeMax / 2)
            {
                Main.NewText("I.. will not... LOSE!", Color.DarkMagenta);
                justStarted = 2;
            }

            if (dist > 1250 && !player.dead)
            {
                npc.Teleport(player.Center + new Vector2(0, -400));
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                if (firstTeleport && timeToNextFraseTeleport <= 0)
                {
                    if (npc.life < npc.lifeMax / 2)
                    {
                        Main.NewText("Come back here, you coward!", Color.DarkMagenta);
                    }
                    else
                    {
                        Main.NewText("You are not getting away.", Color.DarkMagenta);
                    }
                }
                firstTeleport = true;
            }

            if (player.dead || !player.active) return;

            npc.ai[0] += (Main.expertMode && npc.life > npc.lifeMax / 2) ? 5 : 1;
            if (npc.ai[0] < 250)
            {
                Move(new Vector2(150 + npc.width, 0));
            }
            else if (npc.ai[0] < 500)
            {
                Move(new Vector2(0, -(150 + npc.height)));
            }
            else if (npc.ai[0] < 750)
            {
                Move(new Vector2(-(150 + npc.width), 0));
            }
            else
            {
                if (npc.ai[0] > 1000)
                {
                    npc.ai[0] = 0;
                }
                Move(new Vector2(0, -(150 + npc.height)));
            }

            npc.ai[1] -= (npc.life < npc.lifeMax / 2) ? 0.75f : 0.5f;
            if (npc.ai[1] <= 0f)
            {
                FirstAttack();
                SecondAttack();
                if (npc.life < npc.lifeMax / 2)
                {
                    CoolPower();
                }
            }
            else if (npc.ai[1] > 74 && npc.ai[1] < 76)
            {
                if (Main.hardMode)
                {
                    FirstAttack();
                    SecondAttack();
                    if (npc.life < npc.lifeMax * 0.75f)
                    {
                        ThirdAttack();
                    }
                    if (npc.life < npc.lifeMax / 2)
                    {
                        CoolPower();
                    }
                }
                npc.ai[1]--;
            }
            npc.dontTakeDamage = false;

        }

        private void RotateNPCToTarget()
        {
            if (player == null) return;
            Vector2 direction = npc.Center - player.Center;
            float rotation = (float)Math.Atan2(direction.Y, direction.X);
            float rotationtrimed = rotation + ((float)Math.PI * 0.5f);
            npc.rotation = (npc.rotation + rotationtrimed) / 2;
        }

        int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter < 25)
            {
                if (npc.life > npc.lifeMax / 2)
                    frame = 0;
                else
                    frame = 2;
            }
            else if (npc.frameCounter < 50)
            {
                if (npc.life > npc.lifeMax / 2)
                    frame = 1;
                else
                    frame = 3;
            }
            else
            {
                npc.frameCounter = 0;
            }


            npc.frame.Y = frame * frameHeight;
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
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MagicMirror, 1);
                }
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.EoCShield, 1);
                }
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GodEssence"), 5 + Main.rand.Next(20));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LunarBar, 10 + Main.rand.Next(20));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentNebula, 10 + Main.rand.Next(100));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentSolar, 10 + Main.rand.Next(100));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentStardust, 10 + Main.rand.Next(100));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentVortex, 10 + Main.rand.Next(100));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Mannequin, 50);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Womannquin, 50);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TargetDummy, 10);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            float distance = (350 - Magnitude(player.position - position)) / 4;
            scale = 1.5f * (Math.Max(100, distance) / 100);
            return null;
        }
    }
}
