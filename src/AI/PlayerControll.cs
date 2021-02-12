using System;
using System.Collections.Generic;
using EpPathFinding.cs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.Data;

namespace temp1.AI
{
    class PlayerControll : BaseAI
    {
        Mapper<IMovement> _moveMapper;
        Mapper<Direction> _directionMap;
        Mapper<Storage> _storageMap;
        Mapper<MapObject> _moMapper;
        Mapper<AnimatedSprite> _aSpriteMap;
        IBehaviourTreeNode _tree;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _moMapper = cm.Get<MapObject>();
            _moveMapper = cm.Get<IMovement>();
            _storageMap = cm.Get<Storage>();
            _directionMap = cm.Get<Direction>();
            _aSpriteMap = cm.Get<AnimatedSprite>();
            var possibleMoveMap = cm.Get<PossibleMoves>();

            MouseStateExtended mouseState = MouseExtended.GetState();

            _tree = new BehaviourTreeBuilder()
                .Sequence("Start")
                    .Do("Await input", t =>
                    {
                        var newState = MouseExtended.GetState();
                        if (mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
                        && Context.HudState == HudState.Default
                        && !Context.Hud.IsMouseOnHud)
                        {
                            mouseState = newState;
                            return BehaviourTreeStatus.Success;
                        }
                        mouseState = newState;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Selector("Possible Actions")
                        .Do("move", t =>
                        {
                            var point = mouseState.MapPosition(Context.Camera);
                            WalkAction action;
                            if (Context.PointedId == -1)
                                action = new WalkAction(point);
                            else
                                action = new WalkAction(Context.PointedId);
                            var target = _moMapper.Get(Context.PointedId);
                            if(target.Type == GameObjectType.Storage)
                            
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        void CommitAction(int targetId)
        {
            var entity = Context.World.GetEntity(targetId);
            var sprite = entity.Get<AnimatedSprite>();
            var storage = entity.Get<Storage>();
            var mapObj = entity.Get<MapObject>();

            if (mapObj == null)
                return;

            switch (mapObj.Type)
            {
                case GameObjectType.Storage:
                    {
                        sprite.Play("open");
                        Context.Hud.OpenInventory2(storage, _storageMap.Get(EntityId));
                        break;
                    }
                case GameObjectType.Item:
                    {
                        _storageMap.Get(EntityId).Add(entity.Get<ItemStack>());
                        Context.World.DestroyEntity(targetId);
                        break;
                    }
            }
            return;
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}