using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Direction
    {
        private Point from;
        private Point to;
        public float angle;
        
        public Direction(Point from, Point to)
        {
            this.from = from;
            this.to = to;
            var v = from - to;
            if (v.X == 0 && v.Y == 0)
                angle = (float)(Math.PI * 0.5);
            else
                angle = (float)Math.Atan2(v.X, v.Y);
        }

        
    }
}