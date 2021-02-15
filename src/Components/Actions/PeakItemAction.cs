using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Data;

namespace temp1.Components
{
    class PeakItemAction : BaseAction
    {
        public override int PointsTaken => 1;

        private ActionStatus status = ActionStatus.Running;
        public override ActionStatus Status => status;

        int _storageId;
        int _targetId;
        GameContext _context;
        Mapper<Storage> _storageMap;
        Mapper<ItemStack> _stackMap;
        public PeakItemAction(int storageId, int itemId, GameContext context)
        {
            _storageId = storageId;
            _targetId = itemId;
            _context = context;
            var cm = context.World.ComponentManager;
            _storageMap = cm.Get<Storage>();
            _stackMap = cm.Get<ItemStack>();
        }

        public override void Abort()
        {
            status = ActionStatus.Canceled;
        }

        public override void Update(GameTime time)
        { 
            _storageMap.Get(_storageId).Add(_stackMap.Get(_targetId));
            _context.World.DestroyEntity(_targetId);
            status = ActionStatus.Success;
        }
    }
}