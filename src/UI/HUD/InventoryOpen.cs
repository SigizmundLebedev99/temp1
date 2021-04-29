using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1.Components;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class InventoryOpen : Desktop
    {
        ContentControll items;
        Storage storage;
        ControlsFactory _factory;

        public InventoryOpen(Game1 game) : base(game.Batch, SpriteSortMode.FrontToBack)
        {
            _factory = game.Services.GetService<ControlsFactory>();

            var root = new ContentControll();
            root.Size = Size;

            items = new WrapContent();
            items.OffsetFrom = Anchors.Center;

            var panel = _factory.CreatePanel(4);
            panel.ComputeSize(Vector2.Zero, Autosize.Content);
            panel.OffsetFrom = Anchors.Center;

            items.Size = panel.Size - new Vector2(50);

            var area = new InventoryArea(this);
            area.Size = items.Size;
            
            root.Children.Add(area);
            root.Children.Add(panel);
            root.Children.Add(items);
            Root = root;
        }

        internal void AddItem(InventoryItem draggingItem)
        {
            items.Children.Add(draggingItem);
        }

        public void BuildItems(Storage storage)
        {
            this.storage = storage;
            items.Children.Clear();
            for (var i = 0; i < storage.Content.Count; i++)
            {
                var item = _factory.CreateButton<InventoryItem>(7);
                item.Build(storage.Content[i]);
                items.Children.Add(item);
            }
        }
    }
}