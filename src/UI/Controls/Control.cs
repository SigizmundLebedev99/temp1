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

    public enum Autosize
    {
        None,
        Fill,
        Content
    }

    public class Control
    {
        public Vector2 Offset;

        private Vector2? size;

        public Vector2 Size
        {
            get => size ?? DrawingPiece.Bounds.Size.ToVector2();
            set { size = value; }
        }

        public Anchors OffsetFrom = Anchors.TopLeft;

        private IDrawingPiece drawingPiece;
        public IDrawingPiece DrawingPiece { get => drawingPiece ?? (drawingPiece = new NullObjectPiece()); set { drawingPiece = value; } }

        public TextPiece Text { get; }

        public Control()
        {
            var font = Game1.Instance.Content.Load<BitmapFont>("fonts/minor");
            Text = new TextPiece(this, font);
        }

        public virtual void ComputeSize(Vector2 outerSize, Autosize autosize)
        {
            switch (autosize)
            {
                case Autosize.Fill:
                    {
                        Size = outerSize - Offset;
                        break;
                    }
                default:
                    return;
            }
        }

        public virtual void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            DrawingPiece.Update(time);
        }

        public virtual void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            DrawingPiece.Draw(batch, position, depth);
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