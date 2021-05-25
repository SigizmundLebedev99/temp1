using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(RenderingObject))]
    [With(typeof(Direction))]
    class DirectionToAnimationSystem : AEntitySetSystem<GameTime>
    {
        public DirectionToAnimationSystem(World world) : base(world)
        {
        }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var sprite = entity.Get<RenderingObject>();
            var dir = entity.Get<Direction>();
            var angle = dir.angle;
            string direction;

            if (angle >= Math.PI * 0.25 && angle < 0.75 * Math.PI)
                direction = "East";
            else if (Math.Abs(angle) >= Math.PI * 0.75 && Math.Abs(angle) < 1.25 * Math.PI)
                direction = "North";
            else if (angle <= Math.PI * -0.25 && angle > -0.75 * Math.PI)
                direction = "West";
            else
                direction = "South";


            if (!entity.Has<BaseAction>() || !(entity.Get<BaseAction>() is WalkAction))
            {
                sprite.Play("idle" + direction);
                entity.Remove<Direction>();
            }
            else
                sprite.Play("walk" + direction);
        }
    }
}