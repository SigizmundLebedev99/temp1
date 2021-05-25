using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Data;
using temp1.UI;

namespace temp1.AI
{
    class PlayerControll : IBaseAI
    {
        MouseStateExtended mouseState;

        HudContext _hud;
        MapContext _map;

        public PlayerControll() : base()
        {
            var worldContext = GameContext.World;
            _hud = GameContext.Hud;
            _map = GameContext.Map;
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

            if (_map.PathFinder.TryGetPath(position, mapPosition, out var first, out var last, 2f))
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
            && _hud.State == HUDState.Default
            && !_hud.IsMouseOnHud;
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