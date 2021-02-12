using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    abstract class BaseActionSystem : EntityUpdateSystem
    {
        protected GameContext context;
        protected Mapper<ActionPoints> _pointsMapper;
        protected Mapper<TurnOccured> _endOfTurnMapper;

        public BaseActionSystem(AspectBuilder builder, GameContext context) : base(builder)
        {
            this.context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _pointsMapper = mapperService.Get<ActionPoints>();
            _endOfTurnMapper = mapperService.Get<TurnOccured>();
        }

        protected void Complete(BaseAction action, int entityId)
        {
            
            var entity = GetEntity(entityId);
            entity.Detach(action);
            if (action.Status == ActionStatus.Failure)
            {
                if (action.Alternative != null)
                    entity.Attach(action.Alternative);
                return;
            }
            if (action.Status == ActionStatus.Success)
            {
                if (action.After != null)
                    entity.Attach(action.After);
            }

            if(context.GameState == GameState.Peace)
                return;
            var points = entity.Get<ActionPoints>();
            points.Remain -= action.PointsTaken;
            if (points.Remain <= 0)
            {
                points.Remain = points.Max;
                entity.Attach(new TurnOccured());
            }
            
        }
    }
}