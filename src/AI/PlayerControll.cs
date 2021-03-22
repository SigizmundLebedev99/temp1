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

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _walkMapper = cm.Get<WalkAction>();
            _moMapper = cm.Get<MapObject>();
            _baseActionMap = cm.Get<BaseAction>();
        }

        public override void Update(GameTime time)
        {
            var newState = MouseExtended.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && Context.UIState == UIState.Default
            && !Context.UI.IsMouseOnHud)
            {
                mouseState = newState;
                var mapObj = _moMapper.Get(EntityId);
                var pointed = Context.PointedId >= 0 ? _moMapper.Get(Context.PointedId) : null;
                BaseAction after = null;
                if (pointed != null)
                {
                    after = GetAfterAction(pointed, Context.PointedId);
                }
                if (after != null && mapObj.MapPosition == pointed.MapPosition)
                {
                    _baseActionMap.Put(EntityId, after);
                    return;
                }
                if (Context.PathFinder.FindPath(mapObj, mouseState.MapPosition(Context.Camera), out var first, out var last))
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
                return new PeakItemAction(EntityId, id, Context);
            }
            return null;
        }
    }
}