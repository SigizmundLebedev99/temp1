using System;
using System.Linq;
using EpPathFinding.cs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Components;

namespace temp1.AI
{
    class RandomMovement : BaseAI
    {
        private Mapper<IMovement> _moveMapper;
        private IMovement _movement = null;
        private Dot _dot;
        private JumpPointParam _jpParam;
        private IBehaviourTreeNode _tree;
        float _time = 0;
        public RandomMovement(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dot = context.World.GetEntity(entityId).Get<Dot>();
            _moveMapper = cm.Get<IMovement>();
            _jpParam = new JumpPointParam(context.Grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);

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
            var grid = Context.Grid;
            Point point = new Point(random.Next(0, grid.width), random.Next(0, grid.height));
            if (!grid.IsWalkableAt(point.X, point.Y))
                return false;
            var from = _dot.MapPosition;
            if (from == point)
                return false;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(point.X, point.Y));
            var result = JumpPointFinder.FindPath(_jpParam);
            _movement = new PolylineMovement(
                result.Select(e =>
                    new Vector2(e.x * 32 + 16, e.y * 32 + 16))
                    .ToArray(),
                1f);
            _moveMapper.Put(EntityId, _movement);
         

            return true;
        }
    }
}