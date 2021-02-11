using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    interface IExpired
    {
        bool ShouldDestroyEntity { get; }
        bool Update(GameTime time);
    }

    class Timer : IExpired
    {
        private float _seconds;
        private float elapsed = 0;
        private Action action;
        public Timer(float seconds, Action callback, bool shouldDestroy = false)
        {
            _seconds = seconds;
            action = callback;
            ShouldDestroyEntity = shouldDestroy;
        }

        public bool ShouldDestroyEntity { get; }

        public bool Update(GameTime time)
        {
            elapsed += (float)time.ElapsedGameTime.TotalSeconds;
            var result = elapsed >= _seconds;
            if (result)
                action();
            return result;
        }
    }
}