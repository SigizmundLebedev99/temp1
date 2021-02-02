using System;
using System.Linq;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.AI;
using temp1.Components;

namespace temp1.Systems
{
    class AISystem : EntityProcessingSystem
    {
        Mapper<BaseAI> _aiMapper;
        
        public AISystem(BaseGrid grid) : base(Aspect.All(typeof(BaseAI)))
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