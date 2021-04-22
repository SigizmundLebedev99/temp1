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
        Mapper<AnimatedSprite> _aSpriteMap;
        Mapper<Storage> _storageMap;

        HudContext _hud;
        
        public OpenStorageActionSystem(HudContext hud) : base(Aspect.All(typeof(OpenStorageAction)))
        {
            _hud = hud;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _actionMap = mapperService.Get<OpenStorageAction>();
            _aSpriteMap = mapperService.Get<AnimatedSprite>();
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
            var sprite = _aSpriteMap.Get(storageId);
            sprite.Play("open");
            if(GameContext.PlayerId == actorId)
                _hud.OpenInventory2(storage, actor);
            return true;
        }
    }
}