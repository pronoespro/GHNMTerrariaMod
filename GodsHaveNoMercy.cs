using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using GodsHaveNoMercy.UI;
using Microsoft.Xna.Framework;

namespace GodsHaveNoMercy
{
    class GodsHaveNoMercy : Mod
    {
        public static GodsHaveNoMercy instance;

        public UserInterface darknessInterface;
        public static ModHotKey TeleportBackHotKey;
        public static ModHotKey useShadowReleaser;
        public static ModHotKey useShadowPunchDash;
        public UIDarkness darkUI;

        public GodsHaveNoMercy()
        {
            instance = this;
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            TeleportBackHotKey = RegisterHotKey("TeleportBack", "Z");
            useShadowReleaser = RegisterHotKey("UseDarkFist", "G");
            useShadowPunchDash = RegisterHotKey("UltimateAttack", "F");
            base.Load();
            if (!Main.dedServ)
            {
                darknessInterface = new UserInterface();

                darkUI = new UIDarkness();
                darkUI.Activate();
                darkUI.visible = true;
                darknessInterface = new UserInterface();
                darknessInterface.SetState(darkUI);
            }
        }

        public override void Unload()
        {
            TeleportBackHotKey = null;
            useShadowReleaser = null;
            useShadowPunchDash = null;
            instance = null;
            base.Unload();
        }

        #region ui
        private bool StaminaBarUIDraw()
        {
            
            Player player = Main.LocalPlayer;
            DarkPlayer myplayer = player.GetModPlayer<DarkPlayer>();
            darkUI.Initialize();
            /*SpriteBatch sb = Main.spriteBatch;
            if (sb == null) { return true; }
            
            try
            {
                int scr_x = Main.screenWidth - 172;
                int scr_y = 78;
                float alpha = myplayer.Logic.DrainingFX ? 1f : 0.65f;
                int darkness = (int)myplayer.curDarkness;
                int max_darkness = myplayer.Logic.MaxStamina2;
                float fatigue = myplayer.Logic.Fatigue;
                bool is_exercising = myplayer.Logic.IsExercising;
                int threshold = fatigue > 0 ? myplayer.Logic.GetStaminaLossAmountNeededForExercise(this) : -1;

                if (this.Config.CustomStaminaBarPositionX >= 0)
                {
                    scr_x = this.Config.CustomStaminaBarPositionX;
                }
                if (this.Config.CustomStaminaBarPositionY >= 0)
                {
                    scr_y = this.Config.CustomStaminaBarPositionY;
                }

                Logic.DarknessUI.DrawShortStaminaBar(sb, scr_x, scr_y, darkness, max_darkness,alpha, 1f);
            }

            catch (Exception e) { ErrorLogger.Log(e.ToString()); }
            */
            return true;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int idx = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (idx == -1) { return; }

            var main_ui_layer = new LegacyGameInterfaceLayer("Stamina: Main Meter", this.StaminaBarUIDraw, InterfaceScaleType.UI);
            layers.Insert(idx + 1, main_ui_layer);

            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (idx != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "GHNM: Darkness",
                    delegate {
                        if (darkUI.visible)
                        {
                            darknessInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

        }
        public override void UpdateUI(GameTime gameTime)
        {
            if (darkUI.visible)
            {
                darknessInterface?.Update(gameTime);
            }
        }
        #endregion

    }
}
