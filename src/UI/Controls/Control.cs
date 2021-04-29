using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace temp1.UI.Controls
{
    public enum Anchors
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        CenterLeft,
        Center,
        CenterRight
    }

    public enum Autosize
    {
        None,
        Fill,
        Content
    }

    public abstract class Control
    {
        public Vector2 Offset;

        public Vector2 Size;

        public Anchors OffsetFrom = Anchors.TopLeft;

        public virtual void ComputeSize(Vector2 size, Autosize autosize)
        {
            switch (autosize)
            {
                case Autosize.Fill:
                    {
                        Size = size - Offset;
                        break;
                    }
                default:
                    return;
            }
        }

        public abstract void Update(GameTime time, MouseState mouse, Vector2 position);

        public abstract void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0);

        public Vector2 ComputePosition(Vector2 position, Vector2 size)
        {
            return OffsetFrom switch
            {
                Anchors.TopLeft => position + Offset,
                Anchors.TopCenter => position + new Vector2(size.X / 2 - Size.X / 2, 0) + Offset,
                Anchors.TopRight => position + new Vector2(size.X - Size.X, 0) + Offset,
                Anchors.BottomLeft => position + new Vector2(0, size.Y - Size.Y) + Offset,
                Anchors.BottomCenter => position + new Vector2(size.X / 2 - Size.X / 2, size.Y - Size.Y) + Offset,
                Anchors.BottomRight => position + size - Size + Offset,
                Anchors.CenterLeft => position + new Vector2(0, size.Y / 2 - Size.Y / 2) + Offset,
                Anchors.Center => position + (size / 2) - Size / 2 + Offset,
                Anchors.CenterRight => position + new Vector2(size.X - Size.X, size.Y / 2 - Size.Y / 2) + Offset,
                _ => Vector2.Zero
            };
        }
    }
}