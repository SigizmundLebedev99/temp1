using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    // система переключения права хода между акторами
    class TurnBasedCombatSystem : EntityProcessingSystem
    {
        Mapper<AllowedToAct> _allowanceMapper;
        Mapper<TurnOccured> _endOfTurnMapper;
        GameContext _context;

        public TurnBasedCombatSystem(GameContext context) : base(Aspect.All(typeof(AllowedToAct), typeof(TurnOccured)).Exclude(typeof(BaseAction)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _allowanceMapper = mapperService.Get<AllowedToAct>();
            _endOfTurnMapper = mapperService.Get<TurnOccured>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            for (var i = 0; i < _context.Actors.Count; i++)
            {
                var id = _context.Actors[i];
                if (!_endOfTurnMapper.Has(id))
                {
                    _allowanceMapper.Delete(entityId);
                    // ход переходит к следующему актору
                    _allowanceMapper.Put(id, new AllowedToAct());
                    return;
                }
            }

            // если все сходили, очищаем флаги окончания хода
            for (var i = 0; i < _context.Actors.Count; i++)
            {
                var id = _context.Actors[i];
                if (id != entityId)
                    _endOfTurnMapper.Delete(id);
            }
        }
    }
}