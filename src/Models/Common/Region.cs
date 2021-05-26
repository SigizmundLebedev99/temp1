using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Models
{
    class Region
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Region() { }

        public Region(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
        }

        public static implicit operator Rectangle(Region r)
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static implicit operator Region(Rectangle r)
        {
            return new Region(r.X, r.Y, r.Width, r.Height);
        }
    }
}