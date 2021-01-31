using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Dot
    {
        public Vector2 Position;
        public Point MapPosition => (Position / 32).ToPoint();

        public Dot(){}

        public Dot(Vector2 position){
            Position = position;
        }
    }
}