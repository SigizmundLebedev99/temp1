using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class MoveOriginSystem : EntityProcessingSystem
    {
        Mapper<IOriginMove> _moveMapper;

        public MoveOriginSystem() : base(Aspect.All(typeof(IOriginMove)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.Get<IOriginMove>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            _moveMapper.Get(entityId).Update(gameTime);
        }
    }
}