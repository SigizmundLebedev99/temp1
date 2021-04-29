using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using temp1.Data;
using temp1.Factories;
using temp1.UI.Controls;

namespace temp1.UI
{
    class InventoryItem : Button
    {
        public static InventoryItem Dragging;

        public ItemStack Item;

        private DragArea Container;

        private Sprite sprite;
        private Label label;
        private Vector2 MousePosition;

        public InventoryItem()
        {
            this.MouseDown += (s, e) =>
            {
                if (Dragging != null)
                    return;
                Dragging = this;
                this.Depth = 1;
                MousePosition = e.Position.ToVector2();
            };

            this.MouseUp += (s, e) =>
            {
                if (Dragging != this)
                    return;
                Dragging = null;
                this.Depth = 0;
            };
        }

        public void Build(ItemStack stack)
        {
            var factory = GameContext.Game.Services.GetService<ControlsFactory>();

            Item = stack;
            sprite = GameContext.Content.GetSprite(stack.ItemType.Path, stack.ItemType.Region);

            label = factory.CreateLabel(6);
            label.TextAlign = TextAlign.Right;
            label.Text = stack.Count.ToString();
            label.Size = new Vector2(30, 16);
        }

        public void SetContainer(DragArea container)
        {
            if(Container != null)
                Container.RemoveItem(this);
            Container = container;
        }

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position)
        {
            if (Dragging == this)
                position = MousePosition;

            base.Draw(time, batch, position);

            if (sprite != null)
                batch.Draw(sprite.TextureRegion, position + Size / 2 - ((Vector2)sprite.TextureRegion.Size / 2), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, Depth + 0.01f);

            label.Draw(time, batch, position);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            if (Dragging == this)
                position = MousePosition = mouse.Position.ToVector2();
            base.Update(time, mouse, position);
            label.Update(time, mouse, position + new Vector2(5, 25));
        }
    }
}