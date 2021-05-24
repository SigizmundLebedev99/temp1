using DefaultEcs;
using Microsoft.Xna.Framework;
using temp1.Data;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        public Entity Storage;
        public Entity Target;

        public PeakItemAction(Entity storage, Entity target)
        {
            Storage = storage;
            Target = target;
        }

        public override void Start(Entity entity)
        {
            Storage.Get<Storage>().Add(Target.Get<ItemStack>());
            Target.Dispose();
        }

        public override void Update(GameTime time)
        {
            Status = ActionStatus.Success;
        }
    }
}