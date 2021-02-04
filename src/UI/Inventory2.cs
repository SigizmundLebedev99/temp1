using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

namespace temp1.UI
{
    partial class Inventory2
    {
        public int firstSize;
        public int secondSize;
        Texture2D _image;
        public Inventory2(ContentManager content, GameContext context)
        {
            BuildUI();
            _image = content.Load<Texture2D>("animations/chest1");
            closeButton.Click += (s,e) => {
                context.IsInventoryOpen = false;
            };
            BuildInventory(firstPanel, ref firstSize);
        }

        void BuildInventory(VerticalStackPanel inventory, ref int size)
        {
            var scroll = (ScrollViewer) inventory.Parent;
            var raw = (HorizontalStackPanel)inventory.Widgets.LastOrDefault();
            for (var i = 0; i < 45; i++)
            {
                if(size % 4 == 0){
                    raw = inventory.AddChild(new HorizontalStackPanel());
                }
                var item = new Image();
                item.Renderable = new TextureRegion(_image, new Rectangle(0,0, 50, 50));
                item.BorderThickness = new Myra.Graphics2D.Thickness(1);
                raw.AddChild(item);
                size ++;
            }
        }
    }
}