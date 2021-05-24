using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.UI;

namespace temp1.Components
{
    class OpenStorageAction : BaseAction
    {
        public override int PointsTaken => 1;

        public Entity Storage;

        public OpenStorageAction(Entity storage)
        {
            Storage = storage;
        }

        public override void Abort()
        {
            GameContext.Hud.Default();
            Status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { 
            if(GameContext.Hud.State == HUDState.Default)
                Status = ActionStatus.Success;
        }
    }
}