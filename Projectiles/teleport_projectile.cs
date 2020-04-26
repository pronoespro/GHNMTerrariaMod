using Terraria.ModLoader;

namespace GodsHaveNoMercy.Projectiles
{
    class teleport_projectile : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Teleporting spirit");
        }

        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 100000;
        }

    }
}
