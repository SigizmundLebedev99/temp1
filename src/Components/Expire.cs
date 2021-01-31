using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    interface IExpired
    {
        bool ShouldDestroy { get; }
        bool Update(GameTime time);
    }

    class TimeExpired : IExpired
    {
        private float _seconds;
        private float elapsed = 0;
        public TimeExpired(float seconds)
        {
            _seconds = seconds;
        }

        public bool ShouldDestroy => true;

        public bool Update(GameTime time)
        {
            elapsed += (float)time.ElapsedGameTime.TotalSeconds;
            return elapsed >= _seconds;
        }
    }

    class ExpiretionCallback : IExpired
    {
        private float _seconds;
        private float elapsed = 0;
        private Action action;
        public ExpiretionCallback(float seconds, Action callback, bool shouldDestroy = true)
        {
            _seconds = seconds;
            action = callback;
            ShouldDestroy = shouldDestroy;
        }

        public bool ShouldDestroy { get; }

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