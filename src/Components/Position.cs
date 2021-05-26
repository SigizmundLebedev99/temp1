using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Position
    {
        public Vector2 Value = Vector2.Zero;
        public Point GridCell => (Value / 32).ToPoint();
        
        public Position(Vector2 value)
        {
            Value = value;
        }
    }
}