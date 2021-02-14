using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        private ActionStatus status = ActionStatus.Running;
        public override ActionStatus Status => status;

        public int StorageId;
        GameContext _context;
        public PeakItemAction(int storageId, int target, GameContext context)
        {
            StorageId = storageId;
            _context = context;
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { 
            
        }
    }
}