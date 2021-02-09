using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Direction
    {
        public Vector2 previousPosition;
        public Vector2 currentPosition;

        public Direction(Vector2 from)
        {
            previousPosition = from;
            currentPosition = from;
        }

        public float Angle
        {
            get
            {
                var v = currentPosition - previousPosition;
                if (v.X == 0 && v.Y == 0)
                    return (float)(Math.PI * 0.5);
                return (float)Math.Atan2(v.X, v.Y);
            }
        }
    }
}