using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Direction
    {
        public Vector2 PreviousPosition = Vector2.Zero;
        public Vector2 CurrentPosition = Vector2.Zero;

        public float Angle
        {
            get
            {
                var v = CurrentPosition - PreviousPosition;
                if(v.X ==0 && v.Y == 0)
                    return (float)(Math.PI * 0.5);
                return (float)Math.Atan2(v.X,v.Y);
            }
        }
    }
}