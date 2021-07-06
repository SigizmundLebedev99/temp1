using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [WhenAdded(typeof(BaseAction))]
    [WhenChanged(typeof(BaseAction))]
    class TriggerSystem : AEntitySetSystem<GameTime>
    {
        public TriggerSystem(World world) : base(world)
        { }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var baseAction = entity.Get<BaseAction>();
            if(!(baseAction is MoveAction action))
                return;
            var triggers = GameContext.EntitySets.Triggers.GetEntities();
            for (var i = 0; i < triggers.Length; i++)
            {
                var trigger = triggers[i].Get<ITrigger>();
                trigger.Check(triggers[i], action, entity);
            }
        }
    }
}