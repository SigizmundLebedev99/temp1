using System;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    interface IMovement
    {
        bool IsCompleted { get; }
        Vector2 Move();
    }

    class PolylineMovement : IMovement
    {
        Vector2 From;
        Vector2 Target;
        float _change;
        float _speed;
        float k;
        int currentSegment;
        Vector2[] _path;

        public bool IsCompleted => currentSegment == _path.Length - 1 && k >= 1f;

        public PolylineMovement(Vector2[] path, float speed)
        {
            if (path.Length < 2)
                throw new Exception("Path should contain at liast two points");
            _path = path;
            _speed = speed;
            currentSegment = 0;
            MoveTo(path[0], path[1], speed);
        }

        public Vector2 Move()
        {
            k += _change;
            var result = this.Target * k + this.From * (1 - k);
            if(k >= 1 && !IsCompleted){
                MoveTo(_path[currentSegment], _path[currentSegment + 1], _speed);
            }
            return result;
        }

        private void MoveTo(Vector2 from, Vector2 to, float speed)
        {
            k = 0;
            From = from;
            Target = to;
            var v = to - from;
            _change = Math.Clamp(speed / v.Length(), 0, 1);
            currentSegment ++;
        }
    }
}