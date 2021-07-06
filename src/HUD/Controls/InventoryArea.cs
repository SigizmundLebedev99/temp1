using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1.Components;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class InventoryArea : DragArea
    {
        Storage _storage;
        ContentControll _content;
        ControlsFactory _factory;
        
        public InventoryArea(ContentControll content, ControlsFactory factory)
        {
            _content = content;
            _factory = factory;
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth)
        {}

        public override void AddItem(InventoryItem item)
        {
            if(_content.Children.Contains(item))
                return;
            _content.Children.Add(item);
            _storage.Items.Add(item.Item);
            item.SetContainer(this);
        }

        public override void RemoveItem(InventoryItem item)
        {
            _content.Children.Remove(item);
            _storage.Items.Remove(item.Item);
        }

        public void BuildItems(Storage storage)
        {
            _storage = storage;
            _content.Children.Clear();
            for (var i = 0; i < storage.Items.Count; i++)
            {
                var item = _factory.CreateButton<InventoryItem>(7);
                item.Build(storage.Items[i]);
                item.SetContainer(this);
                _content.Children.Add(item);
            }
        }
    }
}