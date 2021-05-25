using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.AI;
using temp1.Components;
using temp1.PathFinding;

namespace temp1.Systems
{
    [With(typeof(IGameAI))]
    [With(typeof(AllowedToAct))]
    class AISystem : AEntitySetSystem<GameTime>
    {   
        public AISystem(World world) : base(world)
        {}

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            entity.Get<IGameAI>().Update(gameTime, entity);
        }
    }
}