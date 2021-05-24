

using DefaultEcs;
using temp1.AI;
using temp1.Components;

namespace temp1
{
    class CombatContext
    {
        private World _world;
        private EntitySet actorsSet;

        public CombatContext(World world)
        {
            _world = world;
            actorsSet = _world.GetEntities().With<IBaseAI>().AsSet();
        }

        public void StartBattle()
        {
            GameContext.GameState = GameState.Combat;
            
            var actors = actorsSet.GetEntities();
            
            foreach (var actor in actors)
            {

            }

            for (var i = 0; i < actors.Length; i++)
            {
                actors[i].Remove<AllowedToAct>();
                actors[i].Remove<TurnOccured>();
                actors[i].Remove<BaseAction>();
            }
            GameContext.Player.Set(new AllowedToAct());
        }
    }
}