using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using temp1.UI.DrawingPieces;
using temp1.UI.Text;

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

    public class Control
    {
        public Vector2 Offset;

        private Vector2? size;
        public Vector2 Size
        {
            get
            {
                return size ?? Background.Size ?? Text.Size;
            }
            set { size = value; this.OnSizeChanged?.Invoke(); }
        }

        public event Action OnSizeChanged;

        public Anchors OffsetFrom = Anchors.TopLeft;

        private IDrawingPiece _background;
        public IDrawingPiece Background { get => _background ?? (_background = new NullObjectPiece()); set { _background = value; } }

        public TextPiece Text { get; }

        public Control()
        {
            var font = Content.Load<BitmapFont>("fonts/minor");
            Text = new TextPiece(this, font);
        }

        public virtual void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            Background.Update(time);
        }

        public virtual void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            Background.Draw(batch, position, depth);
            Text.Draw(batch, position, depth);
        }

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