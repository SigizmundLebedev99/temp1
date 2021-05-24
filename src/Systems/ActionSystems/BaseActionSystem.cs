using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(BaseAction))]
    class BaseActionSystem : AEntitySetSystem<GameTime>
    {
        public BaseActionSystem(World world) : base(world)
        {
        }

        protected override void Update(GameTime time, in Entity entity)
        {
            var action = entity.Get<BaseAction>();

            if (GameContext.GameState == GameState.Combat)
            {
                if (!action.IsChecked)
                {
                    var actionPoints = entity.Get<ActionPoints>();
                    action.IsChecked = true;
                    if (actionPoints.Remain < action.PointsTaken)
                    {
                        entity.Remove<BaseAction>();
                        return;
                    }
                }
            }
            if (action.Status != ActionStatus.Running)
                Complete(action, entity);
            else
                action.Update(time);
        }

        protected void Complete(BaseAction action, Entity entity)
        {
            if (GameContext.GameState == GameState.Combat)
            {
                var actionPoints = entity.Get<ActionPoints>();
                actionPoints.Remain -= action.PointsTaken;
                if (actionPoints.Remain <= 0)
                {
                    actionPoints.Remain = actionPoints.Max;
                    entity.Remove<BaseAction>();
                    entity.Set(new TurnOccured());
                    return;
                }
            }
            if (action.Status == ActionStatus.Canceled)
            {
                if (action.Alternative != null)
                    entity.Set(action.Alternative);
                else
                    entity.Remove<BaseAction>();
            }
            else if (action.Status == ActionStatus.Success)
            {
                if (action.After != null)
                    entity.Set(action.After);
                else
                    entity.Remove<BaseAction>();
            }
        }
    }
}