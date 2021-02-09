using System;
using System.Collections.Generic;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.Data;
using temp1.GridSystem;

namespace temp1.AI
{
    class PlayerControll : BaseAI
    {
        Mapper<IMovement> _moveMapper;
        Mapper<Direction> _directionMap;
        Mapper<Storage> _storageMap;
        Mapper<MapObject> _dotMapper;
        Mapper<AnimatedSprite> _aSpriteMap;

        IBehaviourTreeNode _tree;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dotMapper = cm.Get<MapObject>();
            _moveMapper = cm.Get<IMovement>();
            _storageMap = cm.Get<Storage>();
            _directionMap = cm.Get<Direction>();
            _aSpriteMap = cm.Get<AnimatedSprite>();
            MouseStateExtended state = MouseExtended.GetState();

            _tree = new BehaviourTreeBuilder()
                .Sequence("Start")
                    .Do("Await input", t =>
                    {
                        var newState = MouseExtended.GetState();
                        if (state.LeftButton == ButtonState.Pressed 
                            && newState.LeftButton == ButtonState.Released
                            && Context.HudState == HudState.Default
                            && !Context.Hud.IsMouseOnHud)
                        {
                            state = newState;
                            return BehaviourTreeStatus.Success;
                        }
                        state = newState;
                        return BehaviourTreeStatus.Failure;
                    })
                    .Selector("Possible Actions")
                        .Do("just move", t =>
                        {
                            if (Context.PointedId != -1)
                                return BehaviourTreeStatus.Failure;
                            var pointOnMap = Context.Camera.ScreenToWorld(state.X, state.Y);
                            var to = (pointOnMap / 32).ToPoint();
                            CommitMovement(to);
                            return BehaviourTreeStatus.Success;
                        })
                        .Do("move to target", t =>
                        {
                            var targetId = Context.PointedId;
                            var pos = _dotMapper.Get(targetId).MapPosition;
                            var near = GetNeighbours(pos);
                            CommitMovement(GetBestPath(_dotMapper.Get(EntityId).MapPosition, near), () => CommitAction(targetId));
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        void CommitMovement(Point to)
        {
            if (!Context.MoveGrid.Contains(to.X, to.Y) || !Context.MoveGrid.ValueAt(to.X, to.Y))
                return;
            var from = _dotMapper.Get(EntityId).position;

            var result = PathFinding.FindPath(Context.MoveGrid.GetXY(from), to, Context.MoveGrid);
            CommitMovement(result, null);
        }

        void CommitMovement(Point[] path, Action onComplete)
        {
            if (path.Length < 2)
                return;
            var movement = new PolylineMovement(path, 3f);
            movement.OnComplete = onComplete;
            _moveMapper.Put(EntityId, movement);
            var position = _dotMapper.Get(EntityId).position;
            _directionMap.Put(EntityId, new Direction(position));
        }

        Point[] GetNeighbours(Point point){
            return new Point[]{
                new Point(0,+1) + point,
                new Point(0,-1) + point,
                new Point(+1,0) + point,
                new Point(-1,0) + point
            };
        }

        void CommitAction(int targetId)
        {
            var entity = Context.World.GetEntity(targetId);
            var sprite = entity.Get<AnimatedSprite>();
            var storage = entity.Get<Storage>();
            var mapObj = entity.Get<MapObject>();

            if (mapObj == null)
                return;

            switch (mapObj.type)
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



        Point[] GetBestPath(Point from, Point[] to)
        {
            Point[] min = null;
            int minDist = int.MaxValue;
            for (var i = 0; i < to.Length; i++)
            {
                var path = PathFinding.FindPath(from, to[i], Context.MoveGrid);
                if (path.Length < minDist)
                {
                    minDist = path.Length;
                    min = path;
                }
            }
            return min;
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}