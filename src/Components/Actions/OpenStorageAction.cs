using Microsoft.Xna.Framework;

namespace temp1.Components
{
    class OpenStorageAction : BaseAction
    {
        GameContext _context;
        public override int PointsTaken => 1;

        private ActionStatus status = ActionStatus.Running;
        public override ActionStatus Status => status;

        public int StorageId;
        public OpenStorageAction(int storageId, GameContext context)
        {
            _context = context;
            StorageId = storageId;
        }

        public override void Abort()
        {
            _context.Hud.Default();
            status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { 
            if(_context.Hud.HudState == HudState.Default)
                status = ActionStatus.Success;
        }
    }
}