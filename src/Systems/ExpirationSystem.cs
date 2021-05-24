using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(Expired))]
    class ExpirationSystem : AEntitySetSystem<GameTime>
    {
        public ExpirationSystem(World world) : base(world)
        {
        }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var expire = entity.Get<Expired>();
            if(expire.Update(gameTime)){
                expire.OnCompleted?.Invoke();
                if(expire.ShouldDestroyEntity)
                    entity.Dispose();
                else
                    entity.Remove<Expired>();
            }
        }
    }
}