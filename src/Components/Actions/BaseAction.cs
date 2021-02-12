using Microsoft.Xna.Framework;

namespace temp1.Components
{
    enum ActionStatus
    {
        Started,
        Running,
        Success,
        Failure
    }

    abstract class BaseAction
    {
        public abstract int PointsTaken { get; }

        public abstract ActionStatus Status { get; }

        public abstract void Update(GameTime time);

        public abstract void Abort();

        public BaseAction After;

        public BaseAction Alternative;
    }
}