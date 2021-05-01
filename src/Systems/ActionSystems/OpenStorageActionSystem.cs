using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.UI;

namespace temp1.Systems
{
    class OpenStorageActionSystem : EntityProcessingSystem
    {
        Mapper<OpenStorageAction> _actionMap;
        Mapper<RenderingObject> _spriteMap;
        Mapper<Storage> _storageMap;
        
        public OpenStorageActionSystem() : base(Aspect.All(typeof(OpenStorageAction)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _actionMap = mapperService.Get<OpenStorageAction>();
            _spriteMap = mapperService.Get<RenderingObject>();
            _storageMap = mapperService.Get<Storage>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var entity = GetEntity(entityId);
            var action = entity.Get<OpenStorageAction>();
            entity.Detach<OpenStorageAction>();
            if(Open(action.StorageId, entityId))
                entity.Attach<BaseAction>(action);
        }

        bool Open(int storageId, int actorId){
            var storage = _storageMap.Get(storageId);
            var actor = _storageMap.Get(actorId);
            if(storage == null || actor == null)
                return false;
            var sprite = _spriteMap.Get(storageId);
            sprite.Play("open");
            if(GameContext.PlayerId == actorId)
                GameContext.Hud.OpenChest(storage, actor);
            return true;
        }
    }
}