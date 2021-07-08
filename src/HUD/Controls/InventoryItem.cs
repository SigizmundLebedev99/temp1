using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1.Models;
using temp1.UI.Controls;
using temp1.UI.Text;

namespace temp1.UI
{
    class InventoryItem : MouseControl
    {
        public static InventoryItem Dragging;

        public ItemStack Item;

        public DragArea Container;

        private Sprite sprite;

        private Vector2 MousePosition;

        public InventoryItem()
        {
            this.MouseDown += (s, e) =>
            {
                if (Dragging != null)
                    return;
                Dragging = this;
                MousePosition = GetItemPosition(e);
            };

            this.MouseUp += (s, e) =>
            {
                if (Dragging != this)
                    return;
                Dragging = null;
            };
        }

        public void Build(ItemStack stack)
        {
            Item = stack;
            sprite = Content.GetSprite(stack.ItemType.Sprite);
            if ((stack.ItemType.Flags & ItemTypeFlags.Consumable) == 0)
                return;
            Text.Horizontal = TextAlign.End;
            Text.Vertical = TextAlign.End;
            Text.Value = stack.Count.ToString();
        }

        public void SetContainer(DragArea container)
        {
            if (Container != null && Container != container)
                Container.RemoveItem(this);
            Container = container;
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth)
        {
            if (Dragging == this)
                position = MousePosition;

            depth = Dragging == this ? 0.1f : depth + 0.01f;

            base.DrawingPiece.Draw(batch, position, depth);

            if (sprite != null)
                batch.Draw(sprite.TextureRegion, position + Size / 2 - ((Vector2)sprite.TextureRegion.Size / 2), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth + 0.01f);

            base.Text.Draw(batch, position, depth + 0.001f);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            if (Dragging == this)
                position = MousePosition = GetItemPosition(mouse);
            base.Update(time, mouse, position);
        }

        private Vector2 GetItemPosition(MouseState mouse)
        {
            return mouse.Position.ToVector2() - Size / 2;
        }
    }
}