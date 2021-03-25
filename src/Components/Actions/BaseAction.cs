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
        public bool IsChecked ;

        public abstract int PointsTaken { get; }

        public abstract ActionStatus Status { get; }

        public abstract void Update(GameTime time);

        public abstract void Abort();

        public BaseAction After;

        public BaseAction Alternative;
    }
}