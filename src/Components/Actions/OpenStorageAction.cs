using Microsoft.Xna.Framework;
using temp1.UI;

namespace temp1.Components
{
    class OpenStorageAction : BaseAction
    {
        public override int PointsTaken => 1;

        public int StorageId;

        HudContext _hud;

        public OpenStorageAction(int storageId, HudContext context)
        {
            StorageId = storageId;
            _hud = context;
        }

        public override void Abort()
        {
            _hud.Default();
            Status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { 
            if(_hud.State == HUDState.Default)
                Status = ActionStatus.Success;
        }
    }
}