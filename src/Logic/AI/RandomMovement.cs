using System;
using DefaultEcs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using temp1.Components;
using temp1.Models.Serialization;
using temp1.PathFinding;

namespace temp1.AI
{
    class RandomMovement : IGameAI
    {
        IBehaviourTreeNode _tree;
        float _time = 0;
        Entity entity;

        DefaultPathFinder PathFinder;

        public AIFactory GetFactory()
        {
            return new RandomMovementAIFactory();
        }

        public RandomMovement()
        {
            PathFinder = new DefaultPathFinder();
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
                            if (!TryGetPoint(out var point))
                                return BehaviourTreeStatus.Success;
                            if (PathFinder.TryGetPath(entity, point, out var first, out var last))
                                entity.Set<BaseAction>(first);

                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        bool TryGetPoint(out Point point)
        {
            var random = new Random();
            var grid = GameContext.Map.MovementGrid;
            point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsZeroAt(point.X, point.Y))
                return false;
            return true;
        }

        public void Update(GameTime time, in Entity entity)
        {
            this.entity = entity;
            _tree.Tick(new TimeData());
        }
    }
}