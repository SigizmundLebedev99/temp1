using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    abstract class Expired
    {
        public Action OnCompleted;
        public bool ShouldDestroyEntity ;
        public abstract bool Update(GameTime time);
    }
}