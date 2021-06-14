using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Position
    {
        public Vector2 Value = Vector2.Zero;
        public Point GridCell
        {
            get => Value.GridCell(); set
            {
                Value = value.ToVector2() * 32 + new Vector2(16);
            }
        }

        public Position(Vector2 value)
        {
            Value = value;
        }
    }
}