

using DefaultEcs;
using temp1.AI;
using temp1.Components;

namespace temp1
{
    class CombatContext
    {
        public CombatContext()
        {
        }

        public void StartBattle()
        {
            GameContext.GameState = GameState.Combat;
            
            var actors = GameContext.EntitySets.Actors.GetEntities();

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