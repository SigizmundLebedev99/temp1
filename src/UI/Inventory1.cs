/* Generated by MyraPad at 05.02.2021 15:07:51 */
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using temp1.Components;

namespace temp1.UI
{
	partial class Inventory1
	{
        GameContext _context;
		public Inventory1(GameContext context, HudService hud)
		{
			BuildUI();
            _context = context;
            var sprite = context.World.ComponentManager.Get<AnimatedSprite>().Get(context.PlayerId);
            playerImage.Renderable = sprite.TextureRegion.ToMyra();
            closeButton.Click += (s,e) => hud.Default();
		}

        public void Open(){
            var storage = _context.World.GetEntity(_context.PlayerId).Get<Storage>();
            BuildInventory(firstPanel, storage);
        }

        void BuildInventory(VerticalStackPanel inventory, Storage from)
        {
            inventory.Widgets.Clear();
            var scroll = (ScrollViewer)inventory.Parent;
            var raw = (HorizontalStackPanel)inventory.Widgets.LastOrDefault();
            for (var i = 0; i < from.Content.Count; i++)
            {
                if (i % 4 == 0)
                    raw = inventory.AddChild(new HorizontalStackPanel());
                var slot = from.Content[i];
                var sprite = _context.GetSprite(from.Content[i].ItemType);
                var slotBox = new Grid();
                var item = new Image();
                item.Width = 50;
                item.Height = 50;
                var label = new Label();
                label.Background = null;
                label.TextColor = Color.Black;
                label.Text = slot.Count.ToString();
                label.Margin = new Thickness(6,2);
                item.Renderable = sprite.TextureRegion.ToMyra();
                item.BorderThickness = new Myra.Graphics2D.Thickness(1);
                slotBox.AddChild(item);
                slotBox.AddChild(label);
                slotBox.Background = new SolidBrush(Color.Gray);
                
                slotBox.MouseEntered += (s, e) => slotBox.Border = new SolidBrush(Color.Black);
                slotBox.MouseLeft += (s, e) =>  slotBox.Border = null;
                slotBox.TouchDoubleClick += (s, e) => {
                    from.Content.Remove(slot);
                    BuildInventory(inventory, from);
                    _context.DropItem(slot);
                };
                
                slotBox.BorderThickness = new Thickness(2);
                
                raw.AddChild(slotBox);
            }
        }
	}
}