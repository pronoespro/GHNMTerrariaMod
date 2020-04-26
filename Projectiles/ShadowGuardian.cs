using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace GodsHaveNoMercy.Projectiles
{
    public class ShadowGuardian : MinionAI
    {

        public float speed = 15;
        public bool facing;
        public float distToPlayer = 1;
        public int ShootType;
        public int initDashTimer = 120;
        public int DashTimer;
        public int timespawned;
        public float animTime;
        public int animType = 0;
        public bool rightHand;

        private int[] dusts = { 160, 600, 64, 61, 50, 60, 590 };
        private int DarknessRecover = 100;

        Vector2 positionoffset = new Vector2(-55, 0);
        bool right;

        public override void CreateDust()
        {

        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 25;
            projectile.height = 25;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 0f;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 60;
            projectile.light = 0.05f;
        }

        public void Animate()
        {

            projectile.frameCounter++;

            if (projectile.frameCounter < 5)
            {
                projectile.frame = 0;
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width * 4, projectile.height * 4, DustID.Blood, SpeedX: -projectile.spriteDirection * 3, Scale: 1.75f)];
                dust.noGravity = true;
                dust.alpha = projectile.alpha;
            }
            else if (projectile.frameCounter < 10)
            {
                projectile.frame = 1;
            }
            else if (projectile.frameCounter < 15)
            {
                projectile.frame = 2;
            }
            else if (projectile.frameCounter < 20)
            {
                projectile.frame = 3;
            }
            else
            {
                projectile.frame = 0;
                projectile.frameCounter = 0;
            }

            if (--animTime > 0)
            {
                if (animType == 1)
                {
                    projectile.frame = 4 + ((rightHand) ? 0 : 1);
                }
            }

        }

        public void Shoot(Vector2 shootVel, int projectileType, Vector2 PosOffset = new Vector2())
        {
            int type;

            switch (projectileType)
            {
                default:
                case 1:
                    type = mod.ProjectileType("DarkFist");
                    animTime = 45;
                    animType = 1;
                    rightHand = !rightHand;
                    break;
                case 2:
                    type = mod.ProjectileType("darkDash");
                    break;
                case 3:
                    type = mod.ProjectileType("UltimateDarkDash");
                    break;
            }
            Player player = Main.player[projectile.owner];
            DarkPlayer dp = player.GetModPlayer<DarkPlayer>();
            int proj = Projectile.NewProjectile(projectile.Center.X + PosOffset.X, projectile.Center.Y + PosOffset.Y, shootVel.X, shootVel.Y, type, dp.shadowGuardianDamage, projectile.knockBack, Main.myPlayer, 0f, 0f);

            Main.projectile[proj].timeLeft = 300;
            Main.projectile[proj].netUpdate = true;
            projectile.netUpdate = true;
        }

        public void DarkPunchAttack(Player player, DarkPlayer Player)
        {
            Vector2 offset = projectile.Center - player.Center;
            Vector2 MousePos = new Vector2(Main.mouseX - (Main.screenWidth / 2 + offset.X), Main.mouseY - (Main.screenHeight / 2 + offset.Y));
            Player.ShadowGuardianAttack = 0;
            if (MousePos.X > 0)
            {
                projectile.spriteDirection = (projectile.direction = 1);
            }
            else if (MousePos.X < 0)
            {
                projectile.spriteDirection = (projectile.direction = -1);
            }

            MousePos.Normalize();
            MousePos *= shootSpeed;
            Shoot(MousePos, 1);

            if (Player.shadowPunch.GetLevel() > 1 || Player.UltimateAttackPunch)
            {

                Vector2 Offset = new Vector2(0, 25);
                Shoot(MousePos, 1, Offset);

                Offset = new Vector2(0, -25);
                Shoot(MousePos, 1, Offset);

                if (Player.shadowPunch.GetLevel() > 2 || Player.UltimateAttackPunch)
                {
                    Offset = new Vector2(25, 0);
                    Shoot(MousePos, 1, Offset);

                    Offset = new Vector2(-25, 0);
                    Shoot(MousePos, 1, Offset);
                    if (Player.UltimateAttackPunch)
                    {
                        Offset = new Vector2(-50, 50);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(50, -50);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(50, 50);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(-50, -50);
                        Shoot(MousePos, 1, Offset);

                        //asdf

                        Offset = new Vector2(-50, 0);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(50, 0);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(0, 50);
                        Shoot(MousePos, 1, Offset);

                        Offset = new Vector2(0, -50);
                        Shoot(MousePos, 1, Offset);

                        Player.UltimateAttackPunch = false;
                    }
                }
            }

            projectile.ai[0] = 1;
        }

        public void DarkDash(Player player, DarkPlayer Dplayer)
        {
            if (DashTimer <= 0)
            {
                //Buff
                player.AddBuff(mod.BuffType("DarkDashBuff"), 60);
                //Mouse position
                Vector2 offset = projectile.Center - player.Center;
                Vector2 MousePos = new Vector2(Main.mouseX - (Main.screenWidth / 2 + offset.X), Main.mouseY - (Main.screenHeight / 2 + offset.Y));

                MousePos.Normalize();
                MousePos *= shootSpeed;
                if (Dplayer.UltimateAttackDash)
                {
                    MousePos *= 2;
                    Dplayer.UltimateAttackDash = false;
                    Shoot(player.velocity, 3, player.velocity);
                }
                player.velocity = MousePos;

                DashTimer = initDashTimer / Math.Max(1, (int)(Dplayer.darkDash.GetLevel() * 1.25f - 0.75f));
                Dplayer.DarknessUseTimer = initDashTimer / Math.Max(1, (int)(Dplayer.darkDash.GetLevel() * 1.25f - 0.75f));


                Shoot(player.velocity, 2);
            }
            Dplayer.ShadowGuardianAttack = 0;

        }

        public void TeleportToNearestEnemy(Player player, DarkPlayer DPlayer)
        {
            Vector2 turnedOffset = DPlayer.offset;
            NPC target = new NPC();

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this, false))
                {
                    if (npc.position != new Vector2() && Vector2.Distance(npc.position, player.position) < 400)
                    {
                        Vector2 relativePos = npc.position - player.position;
                        if (relativePos.Length() < (target.position - player.position).Length())
                            target = npc;
                    }
                }
            }

            if (target.position != new Vector2())
            {
                player.Center = target.position + turnedOffset * -target.spriteDirection;
            }

            DPlayer.ShadowGuardianAttack = 0;
            projectile.ai[0] = 30;
        }

        int backAttackTimer = 7;

        public void BackAttack(Player player)
        {

            DarkPlayer dp = player.GetModPlayer<DarkPlayer>();
            NPC target = new NPC();
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this, false))
                {
                    if (npc.position != new Vector2() && Vector2.Distance(npc.position, player.position) < 400)
                    {
                        Vector2 relativePos = npc.position - player.position;
                        if (relativePos.Length() < (target.position - player.position).Length())
                            target = npc;
                    }
                }
            }
            if (target.position != new Vector2())
            {
                Vector2 offset = new Vector2(25 * target.spriteDirection, -15);
                projectile.Center = target.Center + offset;
                backAttackTimer--;
                if (backAttackTimer <= 0)
                {
                    Shoot(target.position - projectile.position, 1);
                    if (backAttackTimer <= -10)
                    {
                        projectile.ai[0] = 30;
                        dp.ShadowGuardianAttack = 0;
                    }
                }
            }
            else
            {
                dp.ShadowGuardianAttack = 0;
                projectile.ai[0] = 30;
            }
        }

        public override void Behavior()
        {
            GodsHaveNoMercy.instance.darknessInterface.SetState(GodsHaveNoMercy.instance.darkUI);

            timespawned++;
            //movement

            Player player = Main.player[projectile.owner];
            DarkPlayer Dplayer = player.GetModPlayer<DarkPlayer>();
            Vector2 pos = LerpPos(player.Center, projectile.Center, speed);


            if (Dplayer.TotalDarkness < Dplayer.MaxDarkness)
            {
                DarknessRecover--;
                if (DarknessRecover <= 0)
                {
                    DarknessRecover = 300;
                    Dplayer.TotalDarkness += Dplayer.darknessRecovery;
                    if (Dplayer.TotalDarkness < 0)
                    {
                        Dplayer.TotalDarkness = 0;
                    }
                    if (Dplayer.TotalDarkness > Dplayer.MaxDarkness)
                    {
                        Dplayer.TotalDarkness = Dplayer.MaxDarkness;
                    }
                }
            }
            else
            {
                DarknessRecover = 100;
            }


            if (player.velocity.X > 0)
            {
                right = true;
                projectile.spriteDirection = (projectile.direction = 1);
            }
            else if (player.velocity.X < 0)
            {
                right = false;
                projectile.spriteDirection = (projectile.direction = -1);
            }
            if (DashTimer > 0)
                projectile.Center = (!right) ? player.position + positionoffset : player.position;
            else
                projectile.Center = (right) ? player.position + positionoffset : player.position;

            Vector2 relativePos = projectile.position - player.position;

            //targeting

            Vector2 targetPos = projectile.position;
            float targetDist = viewDist;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                    if ((distance < targetDist || !target) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        targetDist = distance;
                        targetPos = npc.Center;
                        target = true;
                    }
                }
            }

            //shooting

            if (projectile.ai[0] > 0)
            {
                projectile.ai[0]++;
            }

            if (projectile.ai[0] > shootSpeed)
            {
                projectile.ai[0] = 0f;
                projectile.netUpdate = true;
            }

            if (projectile.ai[0] == 0f)
            {
                switch (Dplayer.ShadowGuardianAttack)
                {
                    case 1:
                        DarkPunchAttack(player, Dplayer);
                        break;
                    case 2:
                        DarkDash(player, Dplayer);
                        break;
                    case 3:
                        TeleportToNearestEnemy(player, Dplayer);
                        break;
                    case 4:
                        BackAttack(player);
                        break;
                    default:
                        break;
                }
            }

            DashTimer--;

            if (player.velocity.Length() == 0 && DashTimer > 20 && Dplayer.lastUseDash)
            {
                DashTimer = 20;
                Dplayer.DarknessUseTimer = -1;
            }

            Animate();

            /*

            if (projectile.ai[1] > 0)
            {
                projectile.ai[1]++;
                if (Main.rand.Next(3) == 0)
                    projectile.ai[1]++;
            }

            if (projectile.ai[1] > shootSpeed)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }

            if (projectile.ai[0] == 0f)
            {
                if (target)
                {
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = 1);
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = -1);
                    }
                    if (projectile.ai[1] == 0f)
                    {
                        projectile.ai[1] = 1f;
                        if (Main.myPlayer == projectile.owner)
                        {
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            Shoot(shootVel);
                        }
                    }
                }
                else
                {
                    if (relativePos.X > 10)
                    {
                        projectile.spriteDirection = (projectile.direction = -1);
                    }
                    else if(relativePos.X<-10)
                    {
                        projectile.spriteDirection = (projectile.direction = 1);
                    }
                }
            }*/
        }

        public void TpPlayerToEnemy()
        {
            Player pl = Main.player[projectile.owner];
            DarkPlayer player = pl.GetModPlayer<DarkPlayer>();
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                if (npc.CanBeChasedBy(this, false))
                {
                    pl.position = npc.position + (player.offset * -npc.spriteDirection);
                    player.ApearedBehind = true;
                    player.ResetApearTimer();
                }
            }
        }

        private Vector2 LerpPos(Vector2 a, Vector2 b, float speed)
        {

            Vector2 c = a - b;

            float cLength = Vector2.Distance(a, b);

            if (cLength >= speed)
            {
                c.Normalize();
                c *= speed;
            }
            else
            {
                c.Normalize();
                c *= cLength;
            }

            return a + c;
        }

        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            DarkPlayer darkPlayer = player.GetModPlayer<DarkPlayer>();
            if (player.dead)
            {
                darkPlayer.summonShadowMinion = false;
            }

            if (!player.HasBuff(mod.BuffType("Shadow")) && timespawned > 30)
            {
                darkPlayer.summonShadowMinion = false;
            }

            if (!darkPlayer.summonShadowMinion && projectile.timeLeft > 4)
            {
                projectile.timeLeft = 2;
            }
            else if (darkPlayer.summonShadowMinion)
            {
                projectile.timeLeft = 150000;
            }
        }

    }
}
