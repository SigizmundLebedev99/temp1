using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace temp1.UI.Controls
{
    public enum TextAlign
    {
        Left,
        Right,
        Center
    }

    public class Label : Control
    {
        public BitmapFont Font;

        public string Text;

        public TextAlign TextAlign = TextAlign.Center;

        private Vector2 textPosition;

        public override void Draw(GameTime time, SpriteBatch batch, Vector2 position, float depth = 0)
        {
            if (Font == null || Text == null)
                return;

            batch.DrawString(Font, Text, textPosition, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);
        }

        public override void Update(GameTime time, MouseState mouse, Vector2 position)
        {
            this.textPosition = ComputeTextPosition(position, out Vector2 textSize);
        }

        public Vector2 ComputeTextPosition(Vector2 position, out Vector2 textSize)
        {
            textSize = Vector2.Zero;

            if (Font == null || Text == null)
                return Vector2.Zero;

            textSize = Font.GetStringRectangle(Text).Size;

            return TextAlign switch
            {
                TextAlign.Left => position + new Vector2(0, Size.Y / 2 - textSize.Y / 2),
                TextAlign.Center => position + new Vector2(Size.X / 2 - textSize.X / 2, Size.Y / 2 - textSize.Y / 2),
                TextAlign.Right => position + new Vector2(Size.X - textSize.X, Size.Y / 2 - textSize.Y / 2),
                _ => Vector2.Zero
            };
        }

        public override void ComputeSize(Vector2 size, Autosize autosize)
        {
            switch (autosize)
            {
                case Autosize.Fill:
                    {
                        Size = size - Offset;
                        break;
                    }
                case Autosize.Content:{
                    if (Font == null || Text == null)
                        Size = Font.MeasureString(Text);
                    break;
                }
                default:
                    return;
            }
        }
    }
}