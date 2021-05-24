using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.AI;
using temp1.Components;

namespace temp1.Systems
{
    // система переключения права хода между акторами
    [With(typeof(AllowedToAct))]
    [With(typeof(TurnOccured))]
    [Without(typeof(BaseAction))]
    class TurnBasedCombatSystem : AEntitySetSystem<GameTime>
    {
        EntitySet actorsSet;
        public TurnBasedCombatSystem(World world) : base(world)
        {
            actorsSet = GameContext.EntitySets.Actors;
        }

        protected override void Update(GameTime state, in Entity entity) 
        {
            var actors = actorsSet.GetEntities();
            for (var i = 0; i < actors.Length; i++)
            {
                var actor = actors[i];
                if (!actor.Has<TurnOccured>())
                {
                    entity.Remove<AllowedToAct>();
                    // ход переходит к следующему актору
                    actor.Set(new AllowedToAct());
                    return;
                }
            }

            // если все сходили, очищаем флаги окончания хода
            for (var i = 0; i < actors.Length; i++)
            {
                var actor = actors[i];
                if (actor != entity)
                    actor.Remove<TurnOccured>();
            }
        }
    }
}