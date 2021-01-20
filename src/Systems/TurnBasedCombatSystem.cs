using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;
using temp1.Services;

using System.Linq;
using System.Collections.Generic;

namespace temp1.Systems
{
    class TurnBasedCombatSystem : EntityProcessingSystem
    {
        private ComponentMapper<BattleStart> _startMapper;
        private ComponentMapper<EndOfTurn> _eotMapper;
        private ComponentMapper<TurnPartitioner> _tpMapper;

        private List<int> combatants = new List<int>(16);

        public TurnBasedCombatSystem() : base(Aspect.One(typeof(BattleStart), typeof(EndOfTurn)))
        {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            _startMapper = mapperService.GetMapper<BattleStart>();
            _eotMapper = mapperService.GetMapper<EndOfTurn>();
            _tpMapper = mapperService.GetMapper<TurnPartitioner>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var start = _startMapper.Get(entityId);
            var endOfTurn = _eotMapper.Get(entityId);
            if (start != null)
            {
                combatants = start.Combatants.OrderBy(c => _tpMapper.Get(c).Speed).ToList();
                foreach(var c in combatants)
                    _tpMapper.Get(c).TurnOccured = false;
                NextTurn(start.First);
            }
            else if (endOfTurn != null)
            {
                if(entityId == combatants.Last()){
                    foreach(var c in combatants)
                        _tpMapper.Get(c).TurnOccured = false;
                    NextTurn(combatants[0]);
                    return;
                }
                for(var i = 0; i < combatants.Count; i ++){
                    if(combatants[i] == entityId){
                        NextTurn(combatants[i + 1]);
                        return;
                    }   
                }
            }
        }

        void NextTurn(int entityId){
            var e = CreateEntity();
            e.Attach(new NextTurn(entityId));
        }
    }
}