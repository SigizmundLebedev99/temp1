using System;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Components;

namespace temp1.AI
{
    class RandomMovement : BaseAI
    {
        Mapper<BaseAction> _actionMap;
        Mapper<WalkAction> _walkMap;
        MapObject _mo;
        IBehaviourTreeNode _tree;
        float _time = 0;
        public RandomMovement(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _mo = context.World.GetEntity(entityId).Get<MapObject>();
            _actionMap = cm.Get<BaseAction>();
            _walkMap = cm.Get<WalkAction>();

            _tree = new BehaviourTreeBuilder()
                .Sequence("start")
                    .Do("check activity", t =>
                    {
                        if (_actionMap.Has(entityId))
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
                            if (Context.PathFinder.FindPath(_mo, point, out var first, out var last))
                            {
                                _walkMap.Put(EntityId, first);
                            }
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        bool TryGetPoint(out Point point)
        {
            var random = new Random();
            var grid = Context.MovementGrid;
            point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsWalkableAt(point.X, point.Y))
                return false;
            return true;
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}