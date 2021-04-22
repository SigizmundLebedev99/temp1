using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Data;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        public int StorageId;
        public int TargetId;

        public PeakItemAction(int storageId, int itemId)
        {
            StorageId = storageId;
            TargetId = itemId;
        }

        public override void Update(GameTime time)
        {
            Status = ActionStatus.Success;
        }
    }
}