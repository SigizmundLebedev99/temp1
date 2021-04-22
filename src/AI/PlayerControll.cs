using FluentBehaviourTree;
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
            if (mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && _hud.State == HUDState.Default
            && !_hud.IsMouseOnHud)
            {
                mouseState = newState;
                var mapObj = _moMapper.Get(EntityId);
                var pointed = GameContext.PointedId >= 0 ? _moMapper.Get(GameContext.PointedId) : null;
                BaseAction after = null;

                if (pointed != null)
                {
                    after = GetAfterAction(pointed, GameContext.PointedId);
                }

                if (after != null && mapObj.MapPosition == pointed.MapPosition)
                {
                    _baseActionMap.Put(EntityId, after);
                    return;
                }

                if (_map.PathFinder.FindPath(mapObj, mouseState.MapPosition(GameContext.Camera), out var first, out var last))
                {
                    (last??first).After = after;
                    _walkMapper.Put(EntityId, first);
                }
            }
            mouseState = newState;
        }

        private BaseAction GetAfterAction(MapObject pointed, int id)
        {
            if (pointed.Type == GameObjectType.Item)
            {
                return new PeakItemAction(EntityId, id);
            }
            return null;
        }
    }
}