using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.UI;

namespace temp1.Components
{
    class OpenStorageAction : BaseAction
    {
        public override int PointsTaken => 1;

        public Entity Storage;

        public override void Start(in Entity entity)
        {
            if (!Storage.Has<Storage>() || !entity.Has<Storage>()){
                Abort();
                return;
            }

            var right = Storage.Get<Storage>();
            var left = entity.Get<Storage>();

            var sprite = Storage.Get<RenderingObject>();
            sprite.Play("open");
            
            GameContext.Hud.OpenChest(left, right);
        }

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
            if (GameContext.Hud.State == HUDState.Default)
                Status = ActionStatus.Success;
        }
    }
}