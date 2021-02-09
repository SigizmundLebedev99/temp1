using System;
using System.Collections.Generic;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    interface IMovement
    {
        bool IsCompleted { get; }
        Vector2 Move();
        public Action OnComplete { get; }
    }

    class PolylineMovement : IMovement
    {
        public Action OnComplete { get; set; }
        public bool IsCompleted => currentSegment == _path.Length - 1 && k >= 1f;

        Vector2 _from;
        Vector2 _to;
        float _change;
        float _speed;
        float k;
        int currentSegment;
        Vector2[] _path;



        public PolylineMovement(Vector2[] path, float speed)
        {
            if (path.Length < 2)
                throw new Exception("Path should contain at liast two points");
            _path = path;
            _speed = speed;
            currentSegment = 0;
            MoveTo(_path[0], _path[1], speed);
        }

        public PolylineMovement(Point[] path, float speed)
        {
            if (path.Length < 2)
                throw new Exception("Path should contain at liast two points");
            _path = new Vector2[path.Length];
            for (var i = 0; i < path.Length; i++)
            {
                var p = path[i];
                _path[i] = new Vector2(p.X * 32 + 16, p.Y * 32 + 16);
            }
            _speed = speed;
            currentSegment = 0;
            MoveTo(_path[0], _path[1], speed);
        }

        public Vector2 Move()
        {
            k += _change;
            var result = this._to * k + this._from * (1 - k);
            if (k >= 1 && !IsCompleted)
            {
                MoveTo(_path[currentSegment], _path[currentSegment + 1], _speed);
            }
            return result;
        }

        private void MoveTo(Vector2 from, Vector2 to, float speed)
        {
            k = 0;
            _from = from;
            _to = to;
            var v = to - from;
            _change = Math.Clamp(speed / v.Length(), 0, 1);
            currentSegment++;
        }
    }

    class FallMovement : IMovement
    {
        public bool IsCompleted => k >= 1;

        public Action OnComplete { get; set; }

        float k = 0;
        Vector2 _from;
        Vector2 _to;

        public FallMovement(Vector2 from, Vector2 to)
        {
            _from = from;
            _to = to;
        }

        public Vector2 Move()
        {
            k += 0.005f;
            var result = _to * k + _from * (1 - k);
            result.Y = result.Y - (float)(Math.Abs(Math.Sin(k * 35) * 20) * (1 - k));
            return result;
        }
    }
}