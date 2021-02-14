using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class PeakItemActionSystem : EntityProcessingSystem
    {
        Mapper<PeakItemAction> _actionMap;
        Mapper<AnimatedSprite> _aSpriteMap;
        Mapper<Storage> _storageMap;

        GameContext _context;

        public PeakItemActionSystem(GameContext context) : base(Aspect.All(typeof(PeakItemAction)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _actionMap = mapperService.Get<PeakItemAction>();
            _aSpriteMap = mapperService.Get<AnimatedSprite>();
            _storageMap = mapperService.Get<Storage>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var entity = GetEntity(entityId);
            var action = entity.Get<PeakItemAction>();
            entity.Detach<OpenStorageAction>();
            if(Open(action.StorageId, entityId))
                entity.Attach<BaseAction>(action);
        }

        bool Open(int storageId, int actorId){
            var storage = _storageMap.Get(storageId);
            var actor = _storageMap.Get(actorId);
            if(storage == null || actor == null)
                return false;
            var sprite = _aSpriteMap.Get(storageId);
            sprite.Play("open");
            if(_context.PlayerId == actorId)
                _context.Hud.OpenInventory2(storage, actor);
            return true;
        }
    }
}