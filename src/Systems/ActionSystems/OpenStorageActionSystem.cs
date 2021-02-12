using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.Systems
{
    class OpenStorageActionSystem : BaseActionSystem
    {
        Mapper<OpenStorageAction> _actionMap;
        Mapper<AnimatedSprite> _aSpriteMap;
        public OpenStorageActionSystem(GameContext context) : base(Aspect.All(typeof(OpenStorageAction)), context)
        {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            base.Initialize(mapperService);
            _actionMap = mapperService.Get<OpenStorageAction>();
        }

        public override void Update(GameTime gameTime)
        {
            for (var i = 0; i < ActiveEntities.Count; i++)
            {
                var action = _actionMap.Get(ActiveEntities[i]);
                var points = _pointsMapper.Get(ActiveEntities[i]);
                if(points.Remain <  action.PointsTaken)
                    action.Abort();
                Complete(action, ActiveEntities[i]);
            }
        }

        void Open(int storageId, int actorId){
            var sprite = _aSpriteMap.Get(storageId);
            sprite.Play("open");
            context.Hud.OpenInventory2(storage, _storageMap.Get(EntityId));
            break;
        }
    }
}