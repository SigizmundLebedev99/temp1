using Microsoft.Xna.Framework;

namespace temp1.Models.Movement
{
    abstract class Movement
    {
        public Vector2 From { get; private set; }
        public Vector2 To { get; private set; }
        public Movement(Vector2 from, Vector2 to)
        {
            From = from;
            To = to;
        }
        public abstract Vector2 Move();
        public abstract bool IsCompleted { get; }
    }
}