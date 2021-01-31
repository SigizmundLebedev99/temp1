using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class ExpirationSystem : EntityProcessingSystem
    {
        ComponentMapper<IExpired> _expireMapper;

        public ExpirationSystem() : base(Aspect.One(typeof(IExpired)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _expireMapper = mapperService.GetMapper<IExpired>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var expire = _expireMapper.Get(entityId);
            if(expire.Update(gameTime)){
                if(expire.ShouldDestroy)
                    DestroyEntity(entityId);
                else
                    _expireMapper.Delete(entityId);
            }
        }
    }
}