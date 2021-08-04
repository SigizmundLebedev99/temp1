using Microsoft.Xna.Framework;

namespace temp1.UI
{
    enum StretchMode
    {
        Fill,
        Repeat,
        Uniform,
        BorderFill,
        BorderRepeat
    }

    class DrawOptions
    {
        public StretchMode Mode { get; init; } = StretchMode.Fill;

        public Vector2? Size;
        public Rectangle? Source;
        public Margin BorderThickness;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin = Vector2.Zero;

        public DrawOptions() { }

        public DrawOptions(Vector2 size, StretchMode mode = StretchMode.Fill)
        {
            Size = size;
            Mode = mode;
        }

        public DrawOptions(Vector2 size, Margin borderThickness, StretchMode mode = StretchMode.BorderFill)
        {
            Size = size;
            BorderThickness = borderThickness;
            Mode = mode;
        }
    }
}