using System;
using DefaultEcs;

namespace temp1.Components
{

    struct Trigger
    {
        public bool ForPlayerOnly;

        public Action<WalkAction, Entity> Invoke;

        public void Check(Entity self, WalkAction action, Entity actionEntity)
        {
            var selfPosition = self.Get<Position>();
            if((action.To / 32).ToPoint() == selfPosition.GridCell)
                Invoke?.Invoke(action, actionEntity);
        }
    }
}