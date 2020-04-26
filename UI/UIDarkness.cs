using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace GodsHaveNoMercy.UI
{
    public class UIDarkness : UIState
    {

        private UIText darkMetter;
        public bool visible;

        public bool alreadyDrawn = false;
        public override void OnInitialize()
        { // 1
            if (!alreadyDrawn)
            {
                alreadyDrawn = !alreadyDrawn;
                UIPanel panel = new UIPanel(); // 2
                panel.BackgroundColor = new Color(0, 0, 0, 0);
                panel.BorderColor = new Color(0, 0, 0, 0);
                panel.Width.Set(150, 0); // 3
                panel.Height.Set(150, 0); // 3
                panel.HAlign = panel.VAlign = 0.5f; // 1
                Append(panel); // 4

                darkMetter = new UIText("");//dp.TotalDarkness.ToString());
                darkMetter.HAlign = 0.5f;
                darkMetter.Top.Set(15, 0);
                darkMetter.TextColor = new Color(150, 0, 0, 255);
                panel.Append(darkMetter);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!visible) return;

            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Player pl = Main.player[Main.myPlayer];
            DarkPlayer dp = pl.GetModPlayer<DarkPlayer>();

            darkMetter.SetText(dp.TotalDarkness.ToString());

            base.Update(gameTime);
        }

    }
}
