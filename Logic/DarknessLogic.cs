using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GodsHaveNoMercy.Logic
{

    partial class DarknessLogic
    {

        private float _darkness;
        public float Darkness
        {
            get
            {
                return this._darkness;
            }
            private set
            {
                float max = (float)this.MaxDarkness2 - this._darkness;
                value = value < 0 ? 0 : value > max ? max : value;
                this._darkness = value;
            }
        }

        public int MaxDarkness { get; private set; }
        public int MaxDarkness2 { get; private set; }

        /*public void PassiveStaminaRegen(GodsHaveNoMercy mymod, Player player)
        {

            //if( this.Player.suffocating || this.Player.breath <= 0 ) { return; }

            if (this.Darkness > 0)
            {
                this.TiredTimer = 0d;
                this.AddStamina(mymod, player, mymod.Config.RechargeRate);
            }
            else
            {
                if (this.TiredTimer >= mymod.Config.ExhaustionDuration)
                {
                    this.TiredTimer = 0d;
                    this.Darkness = 0.0001f;

                    this.AddDarkness(mymod, player, 1);
                }
                else
                {
                    this.TiredTimer += 1d;
                }
            }

        }*/

        public void AddDarkness(Mod mymod, Player player, float amount)
        {
            //amount *= mymod.Config.ScaleAllStaminaRates;
            /*
            if (this.Stamina == 0)
            {
                this.TiredTimer += amount / 2;
            }
            else
            {
                foreach (var hook in this.StaminaChangeHooks)
                {
                    amount = hook(player, StaminaDrainTypes.Recover, amount);
                }

            }*/
            this._darkness += amount;
        }
    }

    public class DarknessUI
    {
        public static void DrawShortStaminaBar(SpriteBatch sb, float x, float y, int darkness, int max_darkness, float alpha, float scale = 1f)
        {
            Texture2D bar = Main.hbTexture1;
            Texture2D maxbar = Main.hbTexture2;
            int tex_width = maxbar.Width;
            int tex_height = maxbar.Height;

            float ratio = darkness / max_darkness;
            if (ratio > 1f) { ratio = 1f; }
            int stamina_bar_length = (int)((float)tex_width * ratio);
            if (stamina_bar_length < 3) { stamina_bar_length = 3; }


            float x_final = x - ((float)(tex_width * 0.5f) * scale);
            float y_final = y;
            float depth = 1f;


            float r = 200;
            float g = 200;
            float b = 200f;
            float a = 255f;

            Color color = new Color((byte)r, (byte)g, (byte)b, (byte)a);
            var pos = new Vector2(x_final, y_final);

            // Underneath bar
            var u_rect = new Rectangle(0, 0, tex_width, tex_height);
            sb.Draw(maxbar, pos, u_rect, color, 0f, new Vector2(), scale, SpriteEffects.None, depth);

            // Overlay bar (stamina)
            var o_rect = new Rectangle(0, 0, (int)((float)tex_width * ratio), bar.Height);
            sb.Draw(bar, pos, o_rect, color, 0f, new Vector2(), scale, SpriteEffects.None, depth);

        }
    }
}