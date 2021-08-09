using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace temp1.UI.DrawingPieces
{
    public interface IDrawingPiece : IDisposable
    {
        Vector2? Size { get; }
        void Update(GameTime time);
        void Draw(SpriteBatch batch, Vector2 position, float depth = 0);
    }
}