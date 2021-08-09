using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using temp1;

namespace temp1.UI.DrawingPieces
{
    class NullObjectPiece : IDrawingPiece
    {
        public Vector2? Size => null;

        public void Dispose()
        {
        }

        public void Draw(SpriteBatch batch, Vector2 position, float depth = 0)
        {
        }

        public void Update(GameTime time)
        {
        }
    }
}