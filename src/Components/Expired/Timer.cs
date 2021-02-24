using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Timer : Expired
    {
        private float _seconds;
        private float elapsed = 0;

        public Timer(float seconds, Action callback, bool shouldDestroy = false)
        {
            _seconds = seconds;
            OnCompleted = callback;
            ShouldDestroyEntity = shouldDestroy;
        }

        public override bool Update(GameTime time)
        {
            elapsed += (float)time.ElapsedGameTime.TotalSeconds;
            return elapsed >= _seconds;
        }
    }
}