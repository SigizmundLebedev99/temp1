using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class ActionPointsSystem : EntityProcessingSystem
    {
        Mapper<ActionOccured> _actionMapper;
        Mapper<ActionPoints> _pointsMapper;
        Mapper<TurnOccured> _endOfTurnMapper;
        GameContext _context;

        public ActionPointsSystem(GameContext context) : base(Aspect.All(typeof(ActionOccured), typeof(ActionPoints)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _actionMapper = mapperService.Get<ActionOccured>();
            _pointsMapper = mapperService.Get<ActionPoints>();
            _endOfTurnMapper = mapperService.Get<TurnOccured>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var action = _actionMapper.Get(entityId);
            _actionMapper.Delete(entityId);
            if (_context.GameState != GameState.Combat)
                return;

            var points = _pointsMapper.Get(entityId);
            points.Remain -= action.PointsTaken;
            if (points.Remain <= 0)
            {
                points.Remain = points.Max;
                _endOfTurnMapper.Put(entityId, new TurnOccured());
            }
        }
    }
}