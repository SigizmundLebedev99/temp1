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

            var mapObj = entity.Get<MapObject>();
            var pointed = GameContext.PointedEntity != null ? GameContext.PointedEntity.Get<MapObject>() : null;
            BaseAction after = null;
            var mapPosition = mouseState.MapPosition(GameContext.Camera);
            
            if (pointed != null)
                after = GetAfterAction(entity, pointed, GameContext.PointedEntity);
            else if (!GameContext.Map.MovementGrid.IsWalkableAt(mapPosition.X, mapPosition.Y))
            {
                return;
            }

            if (_map.PathFinder.TryGetPath(mapObj, mapPosition, out var first, out var last, 2f))
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

        private BaseAction GetAfterAction(Entity entity, MapObject pointed, Entity pointedEntity)
        {
            if ((pointed.Type & GameObjectType.Item) != 0)
                return new PeakItemAction(entity, pointedEntity);
            if ((pointed.Type & GameObjectType.Storage) != 0)
                return new OpenStorageAction(pointedEntity);

            return null;
        }
    }
}