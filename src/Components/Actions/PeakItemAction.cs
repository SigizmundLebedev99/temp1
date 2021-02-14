using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        private ActionStatus status = ActionStatus.Success;
        public override ActionStatus Status => status;

        public int StorageId;
        public PeakItemAction(int storageId, int target)
        {
            StorageId = storageId;
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { }
    }
}