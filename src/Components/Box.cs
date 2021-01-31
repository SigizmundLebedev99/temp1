using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Box
    {
        public Point SelectionBounds;
        public Vector2 Position;
        public Point MapPosition => (Position / 32).ToPoint();
    }
}