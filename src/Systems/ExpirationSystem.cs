using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class ExpirationSystem : EntityProcessingSystem
    {
        Mapper<Expired> _expireMapper;

        public ExpirationSystem() : base(Aspect.All(typeof(Expired)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _expireMapper = mapperService.Get<Expired>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var expire = _expireMapper.Get(entityId);
            if(expire.Update(gameTime)){
                expire.OnCompleted?.Invoke();
                if(expire.ShouldDestroyEntity)
                    DestroyEntity(entityId);
                else
                    _expireMapper.Delete(entityId);
            }
        }
    }
}