using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace temp1.Components
{
    class ActionTrigger : ITrigger
    {
        public Action<MoveAction, Entity> Invoke;

        public virtual void Check(in Entity self, MoveAction action, in Entity actionEntity)
        {
            var selfPosition = self.Get<Position>();
            if (action.To.GridCell() == selfPosition.GridCell)
                Invoke?.Invoke(action, actionEntity);
        }
    }
}