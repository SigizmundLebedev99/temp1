using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Models.Movement;

namespace temp1.Components
{
    class MoveAction : BaseAction
    {
        public Vector2 From => _movement.From;
        public Vector2 To => _movement.To;
        public override int PointsTaken => 1;
        public override ActionStatus Status => status;

        private ActionStatus status = ActionStatus.Running;

        private Position _objToMove;
        private Models.Movement.Movement _movement;

        public MoveAction(Position objToMove, Movement movement)
        {
            _objToMove = objToMove;
            _movement = movement;
        }

        public override void Start(in Entity entity)
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
                    return;
                }
            }
            entity.Set(new Direction(To, From));
        }

        public override void Update(GameTime time)
        {
            _objToMove.Value = _movement.Move();
            if (_movement.IsCompleted)
                status = ActionStatus.Success;
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
        }
    }
}