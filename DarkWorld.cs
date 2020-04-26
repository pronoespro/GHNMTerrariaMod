using GodsHaveNoMercy.Items.Weapons;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace GodsHaveNoMercy
{
    class DarkWorld : ModWorld
    {

        public static int HauntedMansion = 0;

        public bool downedSinnerGod, downedGodessQueen;
        public bool destroyedSpiritAltar;


        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedSinnerGod = downed.Contains("SinnerGod");
            downedGodessQueen = downed.Contains("GodessQueen");
            destroyedSpiritAltar = downed.Contains("destroyedAltar");
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedSinnerGod) downed.Add("SinnerGod");
            if (downedGodessQueen) downed.Add("GodessQueen");
            if (destroyedSpiritAltar) downed.Add("destroyedAltar");

            return new TagCompound {
                {"downed", downed}
            };
        }

        #region worldGen

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {

            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));

            if (LivingTreesIndex != -1)
            {
                tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
                {
                    // We can inline the world generation code like this, but if exceptions happen within this code 
                    // the error messages are difficult to read, so making methods is better. This is called an anonymous method.
                    progress.Message = "Creating castles.";
                    MakeCastle();
                }));
            }

        }

        /*
        public override void PostWorldGen()
        {
            foreach(Chest chest in Main.chest)
            {
                if (WorldGen._genRand.Next(15) == 0)
                {
                    for(int i = 0; i < chest.item.Length; i++)
                    {
                        if(chest.item[i]==new Item())
                        {
                            chest.item[i] = mod.GetItem<cameraObscura>().item;
                        }
                    }
                }
            }
        }
        */

        private void MakeCastle()
        {
            float widthScale = (Main.maxTilesX / 4200f);
            int numberToGenerate = 1;
            for (int k = 0; k < numberToGenerate; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 10000)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
                    if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50)
                    {
                        int j = 0;
                        while (!Main.tile[i, j].active() && (double)j < Main.worldSurface)
                        {
                            j++;
                        }
                        if (Main.tile[i, j].type == TileID.Dirt)
                        {
                            j--;
                            if (j > 150)
                            {
                                bool placementOK = true;
                                for (int l = i - 22; l < i + 22; l++)
                                {
                                    for (int m = j - 12; m < j + 12; m++)
                                    {
                                        if (Main.tile[l, m].active())
                                        {
                                            int type = (int)Main.tile[l, m].type;
                                            if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Cloud || type == TileID.RainCloud)
                                            {
                                                placementOK = false;
                                            }
                                        }
                                    }
                                }
                                if (placementOK)
                                {
                                    success = PlaceRoom(i, j);
                                }
                            }
                        }
                    }
                }
            }
        }

        #region castle shape

        private readonly int[,] castleShape = new int[,]
        {
            {0,2,3,0,2,3,0 },
            {2,1,1,1,1,1,3 },
            {1,0,0,0,0,0,1 },
            {1,0,0,0,0,0,1 },
            {1,0,0,0,0,0,1 },
            {1,0,0,0,0,0,1 },
            {1,0,0,0,0,0,1 },
            {1,4,0,0,0,4,1 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {1,1,1,1,1,1,1 },
            {1,5,1,5,1,5,1 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 }
        };

        private readonly int[,] castleShapeWall = new int[,]
        {
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,1,1,1,1,1,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 }
        };

        #endregion

        #region house rooms

        private readonly int[,] room_one = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,0 },
            {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,5,5,5,5,5,5,5,5,5,5,4,4,4,4,4,4,4,4,4,0,0 },
            {0,0,4,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0 }

            //0 nothing
            //1 dark wood
            //2 dw corner left
            //3 dw corner right
            //4 light wood
            //5 plattform
            //6 platform corner left
            //7 chest
        };

        private readonly int[,] room_two = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,0 },
            {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0 },
            {0,0,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,4,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0 },
            {0,0,4,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0 },
            {0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0 }

            //0 nothing
            //1 dark wood
            //2 dw corner left
            //3 dw corner right
            //4 light wood
            //5 plattform
            //6 platform corner left
        };

        private readonly int[,] room_oneWall = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }

            //0 nothing
            //1 dark wood
        };

        private readonly int[,] room_twoWall = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }

            //0 nothing
            //1 dark wood
        };

        #endregion

        public bool PlaceCastle(int i, int j)
        {

            if (!WorldGen.SolidTile(i, j + 1))
            {
                return false;
            }
            if (Main.tile[i, j].active())
            {
                return false;
            }
            if (j < 150)
            {
                return false;
            }

            for (int y = 0; y < castleShape.GetLength(0); y++)
            {
                for (int x = 0; x < castleShape.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 10 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (castleShape[y, x])
                        {
                            case 1:
                                tile.type = TileID.GrayBrick;
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.GrayBrick;
                                tile.active(true);
                                tile.slope(2);
                                break;
                            case 3:
                                tile.type = TileID.GrayBrick;
                                tile.active(true);
                                tile.slope(1);
                                break;
                            case 4:
                                tile.type = TileID.Torches;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.WoodBlock;
                                tile.active(true);
                                break;
                        }
                        switch (castleShapeWall[y, x])
                        {
                            case 1:
                                tile.wall = WallID.Wood;
                                break;
                        }
                    }
                }
            }
            return true;
        }

        public bool PlaceRoom(int i, int j)
        {

            if (!WorldGen.SolidTile(i, j + 1))
            {
                return false;
            }
            if (Main.tile[i, j].active())
            {
                return false;
            }
            if (j < 150)
            {
                return false;
            }

            int room = WorldGen.genRand.Next(0, 1);

            switch (room)
            {
                case 0:
                    for (int y = 0; y < room_one.GetLength(0); y++)
                    {
                        for (int x = 0; x < room_one.GetLength(1); x++)
                        {
                            int k = i - 22 + x;
                            int l = j - 12 + y;
                            if (WorldGen.InWorld(k, l, 30))
                            {
                                Tile tile = Framing.GetTileSafely(k, l);
                                switch (room_one[y, x])
                                {
                                    case 1:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        break;
                                    case 2:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(2);
                                        break;
                                    case 3:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(1);
                                        break;
                                    case 4:
                                        WorldGen.KillTile(l, k);
                                        //tile.type = TileID.WoodBlock;
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(0);
                                        break;
                                    case 5:
                                        WorldGen.KillTile(l, k);
                                        WorldGen.PlaceTile(l, k, TileID.Platforms, false, false, -1, 19);
                                        tile.halfBrick(true);
                                        tile.active(true);
                                        break;
                                    case 6:
                                        WorldGen.KillTile(l, k);
                                        WorldGen.PlaceTile(l, k, TileID.Platforms, false, false, -1, 19);
                                        tile.slope(1);
                                        break;
                                    case 7:
                                        WorldGen.KillTile(l, k);
                                        PlaceChest(l, k);//,type: (ushort)mod.TileType("SpiritWoodCchest"));
                                        break;
                                    case 0:
                                        tile.type = 0;
                                        tile.active(false);
                                        WorldGen.KillTile(l, k);
                                        break;

                                }
                                switch (room_oneWall[y, x])
                                {
                                    case 1:
                                        WorldGen.KillWall(l, k);
                                        tile.wall = WallID.WhiteDynasty;
                                        break;
                                    case 2:
                                        WorldGen.KillWall(l, k);
                                        tile.wall = WallID.BorealWood;
                                        break;
                                }
                            }
                        }
                    }
                    break;

                case 1:
                    for (int y = 0; y < room_two.GetLength(0); y++)
                    {
                        for (int x = 0; x < room_two.GetLength(1); x++)
                        {
                            int k = i - 22 + x;
                            int l = j - 12 + y;
                            if (WorldGen.InWorld(k, l, 30))
                            {
                                Tile tile = Framing.GetTileSafely(k, l);
                                switch (room_two[y, x])
                                {
                                    case 1:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        break;
                                    case 2:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(2);
                                        break;
                                    case 3:
                                        WorldGen.KillTile(l, k);
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(1);
                                        break;
                                    case 4:
                                        WorldGen.KillTile(l, k);
                                        //tile.type = TileID.WoodBlock;
                                        tile.type = (ushort)mod.TileType("SpiritWood");
                                        tile.active(true);
                                        tile.slope(0);
                                        break;
                                    case 5:
                                        WorldGen.KillTile(l, k);
                                        tile.type = TileID.Platforms;
                                        tile.active(true);
                                        break;
                                    case 6:
                                        WorldGen.KillTile(l, k);
                                        tile.type = TileID.Platforms;
                                        tile.active(true);
                                        tile.leftSlope();
                                        break;
                                    case 7:
                                        WorldGen.KillTile(l, k);
                                        PlaceChest(l, k);//,type: (ushort)mod.TileType("SpiritWoodCchest"));
                                        break;
                                    case 0:
                                        tile.type = 0;
                                        tile.active(false);
                                        WorldGen.KillTile(l, k);
                                        break;

                                }
                                switch (room_twoWall[y, x])
                                {
                                    case 1:
                                        WorldGen.KillWall(l, k);
                                        tile.wall = WallID.WhiteDynasty;
                                        break;
                                    case 2:
                                        WorldGen.KillWall(l, k);
                                        tile.wall = WallID.BorealWood;
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
            return true;
        }

        public static bool PlaceChest(int x, int y, ushort type = 21, bool notNearOtherChests = false, int style = 0)
        {

            bool b = false;
            TileObject toBePlaced;

            if (TileObject.CanPlace(x, y, type, style, 1, out toBePlaced, false))
            {

                bool flag = !(notNearOtherChests && Chest.NearOtherChests(x - 1, y - 1));

                if (flag)
                {
                    b = TileObject.Place(toBePlaced);
                    if (b)
                    {
                        Chest.CreateChest(toBePlaced.xCoord, toBePlaced.yCoord, -1);
                    }
                }

            }
            return b;

            /*
            var chest_num = WorldGen.PlaceChest(x, y);

            if (chest_num != -1)
            {
                Item[] chest = Main.chest[chest_num].item;

                int itemsToPlace = Main.rand.Next(1, 4);
                for (int i = 0; i < itemsToPlace; i++)
                {
                    if (Main.rand.Next(0, 5) == 0)
                    {
                        //God Escence
                        chest[i].SetDefaults(ModContent.ItemType<cameraObscura>());
                    }
                    else if (Main.rand.Next(0, 2) == 0)
                    {
                        //Broken hero's sword
                        chest[i].SetDefaults(ItemID.BrokenHeroSword);
                    }
                    else
                    {
                        //Broken hero's sword
                        chest[i].SetDefaults(ItemID.SwordStatue);
                    }
                }
                Main.chest[chest_num].item = chest;
            }
            */
        }

        #endregion

        public override void TileCountsAvailable(int[] tileCounts)
        {
            HauntedMansion = tileCounts[mod.TileType("SpiritWood")];
        }

        public override void PostWorldGen()
        {
            foreach (Chest chest in Main.chest.Where(c => c != null))
            {
                var tile = Main.tile[chest.x, chest.y];


                if (tile.type == TileID.Containers
                    && (tile.frameX == 3 * 36 || tile.frameX == 4 * 36)
                    && WorldGen.genRand.NextBool(4))
                {
                    foreach (var item in chest.item.Where(x => x != null))
                    {
                        if (tile.type == mod.TileType("SpiritWoodCchest"))
                        {

                            Item[] chestItems = chest.item;

                            int itemsToPlace = Main.rand.Next(1, 4);
                            for (int i = 0; i < itemsToPlace; i++)
                            {
                                if (Main.rand.Next(0, 5) == 0)
                                {
                                    //God Escence
                                    chestItems[i].SetDefaults(ModContent.ItemType<cameraObscura>());
                                }
                                else if (Main.rand.Next(0, 2) == 0)
                                {
                                    //Broken hero's sword
                                    chestItems[i].SetDefaults(ItemID.BrokenHeroSword);
                                }
                                else
                                {
                                    //Broken hero's sword
                                    chestItems[i].SetDefaults(ItemID.SwordStatue);
                                }
                            }
                            chest.item = chestItems;

                        }
                    }
                }


            }
        }
    }
}
