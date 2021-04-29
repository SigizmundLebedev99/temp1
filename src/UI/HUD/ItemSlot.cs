using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using temp1.Data;

namespace temp1.UI
{
    class ItemSlot : DragArea
    {
        public ItemTypeFlags Flags;

        private InventoryItem StoredItem;

        public ItemSlot(ItemTypeFlags type)
        {
            Flags = type;
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
            if (StoredItem != null)
                StoredItem.Update(time, mouse, position);
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            if (StoredItem != null)
                StoredItem.Draw(time, batch, position);
        }

        private void TryAddItem()
        {
            DraggingItem.SetContainer(this);
            StoredItem = DraggingItem;
            DraggingItem = null;
        }

        public override void RemoveItem(InventoryItem item)
        {
            StoredItem = null;
        }

        protected override void AddItem()
        {
            StoredItem = DraggingItem;
        }
    }
}