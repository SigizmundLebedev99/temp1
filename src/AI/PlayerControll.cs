using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Data;
using temp1.Models;
using temp1.UI;

namespace temp1.AI
{
    class PlayerControl : IGameAI
    {
        MouseStateExtended mouseState;

        public AIFactory GetFactory()
        {
            return new PlayerControlAIFactory();
        }

        public void Update(GameTime time, Entity entity)
        {
            var newState = MouseExtended.GetState();
            if (!CanHandleInput(newState))
            {
                mouseState = newState;
                return;
            }

            var position = entity.Get<Position>();
            var pointed = GameContext.PointedEntity != null ? GameContext.PointedEntity.Value.Get<Position>() : null;
            BaseAction after = null;
            var mapPosition = mouseState.MapPosition(GameContext.Camera);
            
            if (pointed != null)
                after = GetAfterAction(entity, GameContext.PointedEntity.Value);
            else if (!GameContext.Map.MovementGrid.IsWalkableAt(mapPosition.X, mapPosition.Y))
            {
                return;
            }

            if (GameContext.Map.PathFinder.TryGetPath(position, mapPosition, out var first, out var last, 2f))
            {
                if (after != null)
                {
                    var action = last ?? first;
                    action.Alternative = after;
                    action.Abort();
                }

                entity.Set<BaseAction>(first);
            }

            mouseState = newState;
        }

        private bool CanHandleInput(MouseStateExtended newState)
        {
            return mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && GameContext.Hud.State == HUDState.Default
            && !GameContext.Hud.IsMouseOnHud;
        }

        private BaseAction GetAfterAction(Entity entity, Entity pointedEntity)
        {
            if(!pointedEntity.Has<GameObjectType>())
                return null;
            var type = pointedEntity.Get<GameObjectType>();
            if ((type & GameObjectType.Item) != 0)
                return new PeakItemAction(entity, pointedEntity);
            if ((type & GameObjectType.Storage) != 0)
                return new OpenStorageAction(pointedEntity);

            return null;
        }
    }
}