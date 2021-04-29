using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using temp1.Data;
using temp1.UI.Controls;

namespace temp1.UI
{
    abstract class DragArea : MouseControl
    {
        protected InventoryItem DraggingItem;

        public DragArea()
        {
            this.MouseEnter += (s, e) =>
            {
                if (InventoryItem.Dragging == null)
                    return;
                DraggingItem = InventoryItem.Dragging;
            };

            this.MouseLeave += (s, e) =>
            {
                DraggingItem = null;
            };

            this.MouseUp += (s, e) =>
            {
                if (DraggingItem == null || InventoryItem.Dragging == null)
                    return;

                AddItem();
                DraggingItem.SetContainer(this);
                DraggingItem = null;
            };
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            base.Update(time, mouse, position);
        }

        protected abstract void AddItem();

        public abstract void RemoveItem(InventoryItem item);
    }
}