using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Direction
    {
        public Vector2 PreviousPosition;
        public Vector2 CurrentPosition;

        public Direction(Vector2 from)
        {
            PreviousPosition = from;
            CurrentPosition = from;
        }

        public float Angle
        {
            get
            {
                var v = CurrentPosition - PreviousPosition;
                if (v.X == 0 && v.Y == 0)
                    return (float)(Math.PI * 0.5);
                return (float)Math.Atan2(v.X, v.Y);
            }
        }
    }
}