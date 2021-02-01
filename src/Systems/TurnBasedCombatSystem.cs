using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;
using System.Linq;
using System.Collections.Generic;

namespace temp1.Systems
{
    class TurnBasedCombatSystem : EntityUpdateSystem
    {
        private Mapper<AllowedToAct> _allowanceMapper;
        private Mapper<TurnPartitioner> _tpMapper;
        private List<int> _combatants = new List<int>();

        public TurnBasedCombatSystem() : base(Aspect.All(typeof(TurnPartitioner), typeof(AllowedToAct)))
        { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _allowanceMapper = mapperService.Get<AllowedToAct>();
            _tpMapper = mapperService.Get<TurnPartitioner>();
        }

        public void StartBattle(List<int> combatants, int startFrom)
        {
            this._combatants = combatants;
            _allowanceMapper.Put(startFrom, new AllowedToAct());
        }

        public override void Update(GameTime gameTime)
        {
            if (_combatants.Count == 0 || ActiveEntities.Count != 0)
                return;
            var order = _combatants.OrderBy(e => _tpMapper.Get(e).Speed);
            var awaiting = order.Where(e => !_tpMapper.Get(e).TurnOccured).ToArray();
            if (awaiting.Length == 0)
            {
                foreach (var e in awaiting)
                    _tpMapper.Get(e).TurnOccured = false;
                _allowanceMapper.Put(order.First(), new AllowedToAct());
                NextTurn(order.First());
            }
            else
            {
                _allowanceMapper.Put(awaiting[0], new AllowedToAct());
                NextTurn(awaiting[0]);
            }
        }

        void NextTurn(int entityId)
        {
            var e = CreateEntity();
            e.Attach(new NextTurn(entityId));
        }
    }
}