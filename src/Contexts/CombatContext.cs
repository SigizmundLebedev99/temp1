
using MonoGame.Extended.Entities;
using temp1.Components;

namespace temp1
{
    class CombatContext
    {
        private WorldContext _world;

        private Mapper<TurnOccured> _turnMapper;
        private Mapper<AllowedToAct> _allowMapper;
        private Mapper<BaseAction> _actionMapper;

        public CombatContext(WorldContext world){
            _world = world;
            _turnMapper = world.GetMapper<TurnOccured>();
            _actionMapper = world.GetMapper<BaseAction>();
            _allowMapper = world.GetMapper<AllowedToAct>();
        }

        public void StartBattle()
        {
            GameContext.GameState = GameState.Combat;
            var actors = _world.Actors;
            for (var i = 0; i < actors.Count; i++)
            {
                _allowMapper.Delete(actors[i]);
                _turnMapper.Delete(actors[i]);
                _actionMapper.Delete(actors[i]);
            }
            _allowMapper.Put(GameContext.PlayerId, new AllowedToAct());
        }
    }
}