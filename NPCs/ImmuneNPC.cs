using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.NPCs
{
    public abstract class ImmuneNPC : ModNPC
    {

        public static string[] cleansingWOrP = new string[]
        {
            "CameraFlash"
        };

        public bool isGhost = false;

        public virtual bool CanBeHit(Projectile Im)
        {

            foreach (string type in cleansingWOrP)
            {
                if (Im.type == mod.ProjectileType(type))
                    return true;
            }

            return false;
        }

        public virtual bool CanBeHit(Item Im)
        {

            foreach (string type in cleansingWOrP)
            {
                if (Im.type == mod.ProjectileType(type))
                    return true;
            }

            return false;
        }

    }
}
