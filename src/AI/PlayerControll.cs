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
        Mapper<MapObject> _dotMapper;
        Mapper<AnimatedSprite> _aSpriteMap;

        JumpPointParam _jpParam;
        IBehaviourTreeNode _tree;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dotMapper = cm.Get<MapObject>();
            _moveMapper = cm.Get<IMovement>();
            _storageMap = cm.Get<Storage>();
            _directionMap = cm.Get<Direction>();
            _aSpriteMap = cm.Get<AnimatedSprite>();
            var possibleMoveMap = cm.Get<PossibleMoves>();

            _jpParam = new JumpPointParam(context.MovementGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
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
                        .Sequence("Combat")
                            .Do("is in battle", t => Context.GameState == GameState.Combat ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure)
                            .Do("try move", t =>
                            {
                                var possibleMoves = possibleMoveMap.Get(EntityId);
                                if (possibleMoves.TryGetPath(mouseState.MapPosition(Context.Camera), out var path))
                                {
                                    CommitMovement(path, null);
                                }
                                return BehaviourTreeStatus.Success;
                            })
                        .End()
                        .Do("move", t =>
                        {
                            var path = FindPath(Context.PointedId, mouseState);
                            if (Context.PointedId != -1)
                                CommitMovement(path, () => CommitAction(Context.PointedId));
                            else
                                CommitMovement(path, null);
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

        List<GridPos> FindPath(int targetId, MouseStateExtended state)
        {
            var from = _dotMapper.Get(EntityId).MapPosition;
            var to = state.MapPosition(Context.Camera);
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y), Context.MovementGrid);
            var path =  JumpPointFinder.FindPath(_jpParam);
            if(targetId != -1)
                path.RemoveAt(path.Count - 1);
            return path;
        }

        void CommitMovement(List<GridPos> path, Action onComplete)
        {
            if (path.Count < 2)
                return;
            var movement = new PolylineMovement(path, 3f);
            movement.OnComplete = onComplete;
            _moveMapper.Put(EntityId, movement);
            var position = _dotMapper.Get(EntityId).Position;
            _directionMap.Put(EntityId, new Direction(position));
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}