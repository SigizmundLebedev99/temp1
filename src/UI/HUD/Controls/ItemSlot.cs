using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using temp1.Models;
using temp1.UI.Controls;

namespace temp1.UI
{
    class ItemSlot : DragArea
    {
        public ItemTypeFlags Flags;

        private InventoryItem StoredItem;
        private Panel _backPanel;

        public ItemSlot(Panel backPanel, ItemTypeFlags type)
        {
            Flags = type;
            _backPanel = backPanel;
            Size = backPanel.Size;
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
            if (StoredItem != null)
                StoredItem.Update(time, mouse, position);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth)
        {
            _backPanel.Draw(time, batch, position, depth);
            if (StoredItem != null)
                StoredItem.Draw(time, batch, position, depth + 0.05f);
        }

        public override void RemoveItem(InventoryItem item)
        {
            StoredItem = null;
        }

        public override void AddItem(InventoryItem item)
        {
            if(!Valid(item))
                return;
            if (StoredItem != null && item != StoredItem)
                item.Container.AddItem(StoredItem);
            StoredItem = item;
            item.SetContainer(this);
        }

        private bool Valid(InventoryItem item)
        {
            return (item.Item.ItemType.Flags & Flags) != 0;
        }
    }
}