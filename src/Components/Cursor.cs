
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Cursor
    {
        public Rectangle Bounds; // offset + bounds size;
        public string SpriteName;

        public Cursor(string spriteName, Rectangle bounds)
        {
            Bounds = bounds;
            SpriteName = spriteName;
        }
    }
}