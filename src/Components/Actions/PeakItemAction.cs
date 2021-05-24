using DefaultEcs;
using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        public Entity Left;
        public Entity Right;

        public PeakItemAction(Entity left, Entity right)
        {
            Left = left;
            Right = right;
        }

        public override void Update(GameTime time)
        {
            Status = ActionStatus.Success;
        }
    }
}