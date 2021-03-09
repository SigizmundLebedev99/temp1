using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class Movement : Expired
    {
        private MapObject _objToMove;

        public Movement(Vector2 from, Vector2 to, MapObject objToMove, float speed)
        {
            this._from = from;
            this._to = to;
            _speed = speed;
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