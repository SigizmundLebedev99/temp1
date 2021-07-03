using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1;

namespace temp1.UI.DrawingPieces
{
    class RectanglePiece : IDrawingPiece
    {
        private Texture2D texture;
        private Rectangle size;

        public RectanglePiece(Color color, Rectangle size)
        {
            this.texture = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new[] { color });
        }

        public int Width => size.Width;

        public Rectangle Bounds => size;

        public int Height => size.Height;

        public void Dispose()
        {
            texture.Dispose();
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
            batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, size.Size.ToVector2(), SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0)
        {
            batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, size.Size.ToVector2() * scale, SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0)
        {
            batch.Draw(texture, position, null, Color.White, rotation, Vector2.Zero, size.Size.ToVector2() * scale, SpriteEffects.None, depth);
        }

        public void Update(GameTime time)
        {}
    }
}