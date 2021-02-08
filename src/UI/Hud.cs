using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

namespace temp1.UI
{
    partial class Hud
    {
        public Hud(ContentManager content, HudService hud)
        {
            BuildUI();
            var texture = content.Load<Texture2D>("images/dragons_blade");
            var inventoryRect = new Rectangle(96, 64, 32, 32);
            inventoryImage.Renderable = new TextureRegion(texture, inventoryRect);
            inventoryImage.TouchUp += (s, e) => hud.OpenInventory1();
            var battleRect = new Rectangle(192,96,32,32);
            battleImage.Renderable = new TextureRegion(texture, battleRect);
            hud.SetCallbacks(inventoryImage);
            SetCallbacks(inventoryImage);
            hud.SetCallbacks(battleImage);
            SetCallbacks(battleImage);
        }

        void SetCallbacks(Widget widget){
            widget.MouseEntered += (s,e) => {
                widget.Width += 4;
                widget.Height += 4;
            };
            widget.MouseLeft += (s,e) => {
                widget.Width -= 4;
                widget.Height -= 4;
            };
        }
    }
}