using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class WalkAction : BaseAction
    {
        public Point from;
        public Point to;
        public override int PointsTaken => 1;
        public override ActionStatus Status => status;
        private ActionStatus status = ActionStatus.Running;

        private MapObject _objToMove;

        public WalkAction(Point from, Point to, MapObject objToMove, float speed)
        {
            this._speed = speed;
            this.from = from;
            this.to = to;
            this._from = from.ToVector2() * 32 + new Vector2(16);
            this._to = to.ToVector2() * 32 + new Vector2(16);
            _objToMove = objToMove;
        }

        public override void Update(GameTime time)
        {
            _objToMove.Position = Move();
            if(k >= 1f)
                status = ActionStatus.Success;
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
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