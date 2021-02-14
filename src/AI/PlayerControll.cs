using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Data;

namespace temp1.AI
{
    class PlayerControll : BaseAI
    {
        Mapper<WalkAction> _walkMapper;
        Mapper<MapObject> _moMapper;
        MouseStateExtended mouseState;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _walkMapper = cm.Get<WalkAction>();
            _moMapper = cm.Get<MapObject>();
        }

        public override void Update(GameTime time)
        {
            var newState = MouseExtended.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && Context.HudState == HudState.Default
            && !Context.Hud.IsMouseOnHud)
            {
                mouseState = newState;
                var mapObj = _moMapper.Get(EntityId);
                if (Context.PathFinder.FindPath(mapObj, mouseState.MapPosition(Context.Camera), out var first, out var last))
                {
                    var target = Context.PointedId;
                    if(target != -1 && _moMapper.Get(target).Type == GameObjectType.Item){
                        last.After = new PeakItemAction(EntityId, target);
                    }
                    _walkMapper.Put(EntityId, first);
                }
            }
            mouseState = newState;
        }
    }
}