using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class OpenStorageAction : BaseAction
    {
        public override int PointsTaken => 1;

        private ActionStatus status = ActionStatus.Success;
        public override ActionStatus Status => status;

        public int StorageId;
        public OpenStorageAction(int storageId)
        {
            StorageId = storageId;
        }

        public override void Abort()
        {
            status = ActionStatus.Failure;
        }

        public override void Update(GameTime time)
        { }
    }
}