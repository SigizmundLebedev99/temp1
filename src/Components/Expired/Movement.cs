using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Movement : Expired
    {
        public Point from;
        public Point to;

        private MapObject _objToMove;

        public Movement(Point from, Point to, MapObject objToMove, float speed)
        {
            this._speed = speed;
            this.from = from;
            this.to = to;
            this._from = from.ToVector2() * 32 + new Vector2(16);
            this._to = to.ToVector2() * 32 + new Vector2(16);
            _objToMove = objToMove;
        }

        public override bool Update(GameTime time)
        {
            _objToMove.Position = Move();
            if (k >= 1f)
                return true;
            return false;
        }

        Vector2 _from;
        Vector2 _to;
        float _speed;
        float k = 0;

        public Vector2 Move()
        {
            k += _speed;
            var result = this._to * k + this._from * (1 - k);
            return result;
        }
    }
}