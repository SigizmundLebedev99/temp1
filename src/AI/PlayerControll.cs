using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Data;
using temp1.UI;

namespace temp1.AI
{
    class PlayerControll : BaseAI
    {
        Mapper<WalkAction> _walkMapper;
        Mapper<BaseAction> _baseActionMap;
        Mapper<MapObject> _moMapper;

        MouseStateExtended mouseState;

        HudContext _hud;
        MapContext _map;

        public PlayerControll(int entityId) : base(entityId)
        {
            var worldContext = GameContext.World;
            _walkMapper = worldContext.GetMapper<WalkAction>();
            _moMapper = worldContext.GetMapper<MapObject>();
            _baseActionMap = worldContext.GetMapper<BaseAction>();
            _hud = GameContext.Hud;
            _map = GameContext.Map;
        }

        public override void Update(GameTime time)
        {
            var newState = MouseExtended.GetState();
            if (!CanHandleInput(newState))
            {
                mouseState = newState;
                return;
            }

            var mapObj = _moMapper.Get(EntityId);
            var pointed = GameContext.PointedId >= 0 ? _moMapper.Get(GameContext.PointedId) : null;
            BaseAction after = null;
            var mapPosition = mouseState.MapPosition(GameContext.Camera);
            
            if (pointed != null)
                after = GetAfterAction(pointed, GameContext.PointedId);
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

                _walkMapper.Put(EntityId, first);
            }

            mouseState = newState;
        }

        private bool CanHandleInput(MouseStateExtended newState)
        {
            return mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && _hud.State == HUDState.Default
            && !_hud.IsMouseOnHud;
        }

        private BaseAction GetAfterAction(MapObject pointed, int id)
        {
            if ((pointed.Type & GameObjectType.Item) != 0)
                return new PeakItemAction(EntityId, id);
            if ((pointed.Type & GameObjectType.Storage) != 0)
                return new OpenStorageAction(GameContext.PointedId);

            return null;
        }
    }
}