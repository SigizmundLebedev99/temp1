using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace temp1.UI.DrawingPieces
{
    public interface IDrawingPiece : IDisposable
    {
        int Width { get; }
        Rectangle Bounds { get; }
        int Height { get; }
        void Update(GameTime time);
        void Draw(SpriteBatch batch, Vector2 position, float depth = 0);
        void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float depth = 0);
        void Draw(SpriteBatch batch, Vector2 position, Vector2 scale, float rotation, float depth = 0);
    }
}