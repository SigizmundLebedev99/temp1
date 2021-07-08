using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1;
using temp1.UI.Controls;

namespace temp1.UI.DrawingPieces
{
    class BackgroundColorPiece : IDrawingPiece
    {
        private Texture2D texture;
        Control control;

        public BackgroundColorPiece(Color color, Control control)
        {
            this.texture = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new[] { color });
            this.control = control;
        }

        public int Width => 0;

        public Rectangle Bounds => new Rectangle();

        public int Height => 0;

        public void Dispose()
        {
            texture.Dispose();
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
            batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, control.Size, SpriteEffects.None, depth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0)
        {
            this.Draw(batch, position);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0)
        {
            this.Draw(batch, position);
        }

        public void Update(GameTime time)
        {}
    }
}