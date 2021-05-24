using System;
using DefaultEcs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.AI
{
    class RandomMovement : IBaseAI
    {
        MapObject _mo;
        IBehaviourTreeNode _tree;
        float _time = 0;
        Entity entity;
        
        public RandomMovement()
        {
            var world = GameContext.World;
            var mapContext = GameContext.Map;

            _tree = new BehaviourTreeBuilder()
                .Sequence("start")
                    .Do("check activity", t =>
                    {
                        if (entity.Has<BaseAction>())
                            return BehaviourTreeStatus.Running;
                        return BehaviourTreeStatus.Success;
                    })
                    .Sequence("createPath")
                        .Do("await", t =>
                        {
                            if (_time <= 1)
                            {
                                _time += 0.007f;
                                return BehaviourTreeStatus.Running;
                            }
                            _time = 0;
                            return BehaviourTreeStatus.Success;
                        })
                        .Do("create", t =>
                        {
                            if (!TryGetPoint(out var point, mapContext))
                                return BehaviourTreeStatus.Success;
                            if (mapContext.PathFinder.TryGetPath(_mo, point, out var first, out var last, 1f))
                                entity.Set<BaseAction>(first);
                            
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        bool TryGetPoint(out Point point, MapContext map)
        {
            var random = new Random();
            var grid = map.MovementGrid;
            point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsZeroAt(point.X, point.Y))
                return false;
            return true;
        }

        public void Update(GameTime time, Entity entity)
        {
            this.entity = entity;
            _tree.Tick(new TimeData());
        }
    }
}