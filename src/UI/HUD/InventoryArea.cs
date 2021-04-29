using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1.Components;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class InventoryArea : DragArea
    {
        InventoryOpen _inventoryOpen;
        
        public InventoryArea(InventoryOpen inventoryOpen)
        {
            _inventoryOpen = inventoryOpen;
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {}

        protected override void AddItem()
        {
            _inventoryOpen.AddItem(DraggingItem);
        }

        public override void RemoveItem(InventoryItem item)
        {
            _inventoryOpen.AddItem(item);
        }
    }
}