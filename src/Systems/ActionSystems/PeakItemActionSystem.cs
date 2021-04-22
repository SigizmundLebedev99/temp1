using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.Data;

namespace temp1.Systems
{
    class PeakItemActionSystem : EntityProcessingSystem
    {
        Mapper<Storage> _storageMap;
        Mapper<ItemStack> _stackMap;

        public PeakItemActionSystem() : base(Aspect.All(typeof(PeakItemAction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _storageMap = mapperService.Get<Storage>();
            _stackMap = mapperService.Get<ItemStack>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var entity = GetEntity(entityId);
            var action = entity.Get<PeakItemAction>();
            
            entity.Detach<PeakItemAction>();
            entity.Attach<BaseAction>(action);
        
            _storageMap.Get(action.StorageId).Add(_stackMap.Get(action.TargetId));
            World.DestroyEntity(action.TargetId);
        }
    }
}