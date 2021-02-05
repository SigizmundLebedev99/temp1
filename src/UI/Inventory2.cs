using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using temp1.Components;

namespace temp1.UI
{
    partial class Inventory2
    {
        public int firstSize;
        public int secondSize;
        GameContext _context;
        public Inventory2(GameContext context)
        {
            BuildUI();
            closeButton.Click += (s, e) =>
            {
                context.GameState = GameState.Default;
            };
            _context = context;
        }

        public void Build(Storage chest)
        {
            firstPanel.Widgets.Clear();
            firstSize = 0;
            BuildInventory(firstPanel, chest, ref firstSize);
        }

        void BuildInventory(VerticalStackPanel inventory, Storage from, ref int size)
        {
            var scroll = (ScrollViewer)inventory.Parent;
            var raw = (HorizontalStackPanel)inventory.Widgets.LastOrDefault();
            for (var i = 0; i < from.Content.Count; i++)
            {
                if (size % 4 == 0)
                {
                    raw = inventory.AddChild(new HorizontalStackPanel());
                }
                var slot = from.Content[i];
                var sprite = _context.GetSprite(from.Content[i].ItemType.image);
                var slotBox = new Grid();
                var item = new Image();
                var label = new Label();
                label.Background = null;
                label.TextColor = Color.Black;
                label.Text = slot.Count.ToString();
                label.Margin = new Thickness(6,2);
                item.Renderable = new TextureRegion(sprite.TextureRegion.Texture);
                item.BorderThickness = new Myra.Graphics2D.Thickness(1);
                slotBox.AddChild(item);
                slotBox.AddChild(label);
                slotBox.Background = new SolidBrush(Color.Gray);
                slotBox.MouseEntered += (s, e) =>
                {
                    slotBox.Border = new SolidBrush(Color.Black);
                };
                slotBox.MouseLeft += (s, e) =>
                {
                    slotBox.Border = null;
                };
                slotBox.BorderThickness = new Thickness(2);
                raw.AddChild(slotBox);
                size++;
            }
        }
    }
}