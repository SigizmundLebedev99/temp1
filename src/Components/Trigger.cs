using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace temp1.Components
{
    interface ITrigger
    {
        void Check(in Entity self, WalkAction action, in Entity actionEntity);
    }

    class ActionTrigger : ITrigger
    {
        public Action<WalkAction, Entity> Invoke;

        public virtual void Check(in Entity self, WalkAction action, in Entity actionEntity)
        {
            var selfPosition = self.Get<Position>();
            if (action.To.GridCell() == selfPosition.GridCell)
                Invoke?.Invoke(action, actionEntity);
        }
    }

    class Portal : ITrigger
    {
        private Rectangle Zone;
        private Point Transition;
        // if null, player stays on the map
        private string DestinationMap;

        public Portal(TiledMapObject from)
        {
            Zone = new Rectangle();
            Transition = new Point();
            DestinationMap = null;

            if (from.Properties.TryGetValue("destination", out var destination))
                DestinationMap = destination;
            if (
                from.Properties.TryGetValue("player_transition_x", out var xProp)
                && from.Properties.TryGetValue("player_transition_y", out var yProp)
                && int.TryParse(xProp, out int x)
                && int.TryParse(yProp, out int y))
                Transition = new Point(x, y);
            if (from is TiledMapRectangleObject rect)
                Zone = new Rectangle(rect.Position.GridCell(), (Point)(rect.Size.Width == 0 ? new Size2(1, 1) : rect.Size / 32));
        }

        public void Check(in Entity self, WalkAction action, in Entity actionEntity)
        {
            if (!actionEntity.Has<Player>())
                return;
            if (!Zone.Contains(action.To.GridCell()))
                return;
            var position = actionEntity.Get<Position>();
            
            action.After = new SingleAction((_) =>
            {
                position.GridCell = position.GridCell + Transition;
                if (!string.IsNullOrEmpty(DestinationMap))
                    GameContext.LoadMap(DestinationMap);
            });
        }
    }
}