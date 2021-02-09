using System;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Components;
using temp1.GridSystem;

namespace temp1.AI
{
    class RandomMovement : BaseAI
    {
        Mapper<IMovement> _moveMapper;
        Mapper<Direction> _directionMap;
        IMovement _movement = null;
        MapObject _dot;

        IBehaviourTreeNode _tree;
        float _time = 0;
        public RandomMovement(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dot = context.World.GetEntity(entityId).Get<MapObject>();
            _moveMapper = cm.Get<IMovement>();
            _directionMap = cm.Get<Direction>();

            _tree = new BehaviourTreeBuilder()
                .Sequence("start")
                    .Do("checkMovement", t =>
                    {
                        if(_movement == null || _movement.IsCompleted)
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Running;
                    })
                    .Sequence("createPath")
                        .Do("await", t =>{
                            if(_time <= 1){
                                _time += 0.007f;
                                return BehaviourTreeStatus.Running;
                            }
                            _time = 0;
                            return BehaviourTreeStatus.Success;
                        })
                        .Do("create", t =>
                        {
                            SetMovement();
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }

        bool SetMovement(){
            var random = new Random();
            var grid = Context.MoveGrid;
            Point point = new Point(random.Next(0, grid.Width), random.Next(0, grid.Height));
            if (!grid.ValueAt(point.X, point.Y))
                return false;
            var from = _dot.MapPosition;
            if (from == point)
                return false;

            var result = PathFinding.FindPath(from, point, grid);
            if (result.Length < 2)
                return false;
            _movement = new PolylineMovement(result, 1f);
            _moveMapper.Put(EntityId, _movement);
            _directionMap.Put(EntityId, new Direction(_dot.position));

            return true;
        }
    }
}