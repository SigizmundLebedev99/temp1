using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class WalkAction : BaseAction
    {
        public Point To;
        public int TargetId;

        public override int PointsTaken => _points;

        public override ActionStatus Status => status;

        private ActionStatus status = ActionStatus.Started;
        private int _points;
        private IMovement _movement;

        public WalkAction(Point to)
        {
            To = to;
            TargetId = -1;
        }

        public WalkAction(int targetId)
        {
            TargetId = targetId;
        }

        public void SetInfo(int points, IMovement movement)
        {
            _points = points;
            _movement = movement;
        }

        public override void Abort()
        {
            status = ActionStatus.Failure;
        }

        public override void Update(GameTime time)
        {
            if(_movement.IsCompleted)
                status = ActionStatus.Success;
        }
    }
}