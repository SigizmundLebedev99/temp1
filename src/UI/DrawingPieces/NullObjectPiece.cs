using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1;

namespace temp1.UI.DrawingPieces
{
    class NullObjectPiece : IDrawingPiece
    {
        public int Width => 0;

        public Rectangle Bounds => new Rectangle();

        public int Height => 0;

        public void Dispose()
        {
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0)
        {
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0)
        {
        }

        public void Update(GameTime time)
        {
        }
    }
}