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
                        // .Sequence("Combat")
                        //     .Do("Is in battle", t => Context.GameState == GameState.Combat ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure)
                        //     .Do("Estimate", t =>
                        //     {

                        //     })
                        // .End()
                        .Do("move", t =>
                        {
                            var path = FindPath(Context.PointedId, mouseState);
                            if(Context.PointedId != -1)
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

        List<GridPos> FindPath(Point to)
        {
            var from = _dotMapper.Get(EntityId).MapPosition;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y), Context.MovementGrid);
            return JumpPointFinder.FindPath(_jpParam);
        }

        List<GridPos> FindPath(int targetId, MouseStateExtended state)
        {
            var from = _dotMapper.Get(EntityId).MapPosition;
            if(targetId == -1){
                var pointOnMap = Context.Camera.ScreenToWorld(state.X, state.Y);
                var to = (pointOnMap / 32).ToPoint();
                return FindPath(to);
            }
            var pos = _dotMapper.Get(targetId).MapPosition;
            return GetBestPath(pos);
        }

        List<GridPos> GetBestPath(Point pos)
        {
            var node = Context.MovementGrid.GetNodeAt(pos.X, pos.Y);
            var to = Context.MovementGrid.GetNeighbors(node, DiagonalMovement.Never);
            if (to.Count == 1)
                return new List<GridPos>{
                    new GridPos(to[0].x, to[0].y)
                };
            List<GridPos> min = null;
            int minDist = int.MaxValue;
            for (var i = 0; i < to.Count; i++)
            {
                var path = FindPath(new Point(to[i].x, to[i].y));
                if (path.Count < minDist)
                {
                    minDist = path.Count;
                    min = path;
                }
            }
            return min;
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