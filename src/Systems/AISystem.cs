using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.AI;
using temp1.Components;
using temp1.PathFinding;

namespace temp1.Systems
{
    class AISystem : EntityProcessingSystem
    {
        Mapper<BaseAI> _aiMapper;
        
        public AISystem() : base(Aspect.All(typeof(BaseAI), typeof(AllowedToAct)))
        {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            _aiMapper = mapperService.Get<BaseAI>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            _aiMapper.Get(entityId).Update(gameTime);
        }
    }
}