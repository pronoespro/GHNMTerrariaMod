using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Projectiles
{
    class darkDash : ModProjectile
    {

        public int curFrame;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 150;
            projectile.height = 150;
            projectile.scale = 1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = 50;
            projectile.light = 0.5f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.timeLeft = 200;
            projectile.damage = 200;
            projectile.light = 1f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            DarkPlayer Dplayer = Main.player[projectile.owner].GetModPlayer<DarkPlayer>();
            Dplayer.darkDash.GainExp(1);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center + player.velocity * 2;
            projectile.velocity = player.velocity / 2;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            //destroy if velocity is too low
            if (player.velocity.Length() < 10)
            {
                projectile.timeLeft = 1;
            }

            //animate
            projectile.ai[0]++;
            if (projectile.ai[0] < 10)
            {
                projectile.frame = 0;
                if (curFrame != 0)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Granite, SpeedX: projectile.velocity.X, SpeedY: projectile.velocity.Y, newColor: Color.Black, Scale: 1.1f);
                }
                curFrame = 0;
            }
            else if (projectile.ai[0] < 20)
            {
                projectile.frame = 1;
                if (curFrame != 0)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Granite, SpeedX: projectile.velocity.X, SpeedY: projectile.velocity.Y, newColor: Color.Black, Scale: 1.1f);
                }
                curFrame = 1;
            }
            else if (projectile.ai[0] < 30)
            {
                projectile.frame = 2;
                if (curFrame != 0)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Granite, SpeedX: projectile.velocity.X, SpeedY: projectile.velocity.Y, newColor: Color.Black, Scale: 1.1f);
                }
                curFrame = 2;
            }
            else
            {
                projectile.frame = 0;
                projectile.ai[0] = 0;
            }
        }

    }
}
