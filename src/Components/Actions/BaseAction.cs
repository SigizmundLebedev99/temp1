using DefaultEcs;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    enum ActionStatus
    {
        Running,
        Success,
        Canceled
    }

    abstract class BaseAction
    {
        public bool IsChecked;

        public abstract int PointsTaken { get; }

        public virtual ActionStatus Status { get; set; }

        public virtual void Start(in Entity entity) { }

        public virtual void Update(GameTime time) { }

        public virtual void Abort() { Status = ActionStatus.Canceled; }

        public BaseAction After;

        public BaseAction Alternative;
    }
}