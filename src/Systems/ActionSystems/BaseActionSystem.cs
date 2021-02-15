using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class BaseActionSystem : EntityProcessingSystem
    {
        GameContext context;
        Mapper<ActionPoints> _pointsMapper;
        Mapper<TurnOccured> _endOfTurnMapper;
        Mapper<BaseAction> _actionMapper;

        public BaseActionSystem(GameContext context) : base(Aspect.All(typeof(BaseAction)))
        {
            this.context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _pointsMapper = mapperService.Get<ActionPoints>();
            _endOfTurnMapper = mapperService.Get<TurnOccured>();
            _actionMapper = mapperService.Get<BaseAction>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var action = _actionMapper.Get(entityId);
            if (context.GameState == GameState.Combat)
            {
                if (!action.IsChecked)
                {
                    var actionPoints = _pointsMapper.Get(entityId);
                    action.IsChecked = true;
                    if (actionPoints.Remain < action.PointsTaken)
                    {
                        _actionMapper.Delete(entityId);
                        return;
                    }
                }
            }
            if (action.Status != ActionStatus.Running)
                Complete(action, entityId);
            else
                action.Update(gameTime);
        }

        protected void Complete(BaseAction action, int entityId)
        {
            var entity = GetEntity(entityId);
            if (context.GameState == GameState.Combat)
            {
                var actionPoints = _pointsMapper.Get(entityId);
                actionPoints.Remain -= action.PointsTaken;
                if (actionPoints.Remain <= 0)
                {
                    actionPoints.Remain = actionPoints.Max;
                    _actionMapper.Delete(entityId);
                    _endOfTurnMapper.Put(entityId, new TurnOccured());
                    return;
                }
            }
            if (action.Status == ActionStatus.Canceled)
            {
                if (action.Alternative != null)
                    entity.Attach((object)action.Alternative);
                else
                    entity.Detach<BaseAction>();
            }
            else if (action.Status == ActionStatus.Success)
            {
                if (action.After != null)
                    entity.Attach((object)action.After);
                else
                    entity.Detach<BaseAction>();
            }
        }
    }
}