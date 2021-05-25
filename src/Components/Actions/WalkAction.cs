using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Data;

namespace temp1.Components
{
    class WalkAction : BaseAction
    {
        public Vector2 From;
        public Vector2 To;
        public override int PointsTaken => 1;
        public override ActionStatus Status => status;

        private ActionStatus status = ActionStatus.Running;

        private Position _objToMove;

        public WalkAction(Vector2 from, Vector2 to, Position objToMove, float speed)
        {
            this._speed = speed / (to - from).Length();
            this.From = from;
            this.To = to;
            _objToMove = objToMove;
        }

        public override void Start(Entity entity)
        {
            var mapObjects = GameContext.EntitySets.MapObjects.GetEntities();

            for (var i = 0; i < mapObjects.Length; i++)
            {
                var mapObj = mapObjects[i].Get<Position>();
                if (mapObj.Value != To)
                    continue;
                else if (mapObjects[i].Has<Blocking>())
                {
                    Abort();
                    break;
                }
            }
            entity.Set(new Direction(To, From));
            GameContext.World.Publish((this, entity));
        }

        public override void Update(GameTime time)
        {
            _objToMove.Value = Move();
            if (k >= 1f)
            {
                status = ActionStatus.Success;
            }
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
        }

        float _speed;
        float k = 0;

        public Vector2 Move()
        {
            k += _speed;
            var result = this.To * k + this.From * (1 - k);
            return result;
        }
    }
}