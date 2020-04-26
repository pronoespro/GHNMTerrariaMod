using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace GodsHaveNoMercy.Tiles
{
    public class SpiritWood : ModTile
    {

        public override void SetDefaults()
        {

            TileObjectData.newTile.FullCopyFrom(TileID.Ebonwood);
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileSolid[Type] = true;
            minPick = 999;
            AddMapEntry(new Color(50, 50, 50));

        }

    }
}
