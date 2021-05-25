using System;
using Microsoft.Xna.Framework;
using temp1.Data;

namespace temp1.Models
{
    class RenderObjectInfo
    {
        public string Path;
        public Origin Origin;
        public Region Region;
    }

    class Origin
    {
        public float X;
        public float Y;

        internal Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}